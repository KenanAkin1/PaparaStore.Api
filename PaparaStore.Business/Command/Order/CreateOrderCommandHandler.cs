using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Command.Order;
using PaparaStore.Business.Cqrs;
using PaparaStore.Business.Notification;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Security.Claims;
using System.Text;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<OrderResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly INotificationService notificationService;
    private readonly PaymentHandler paymentHandler;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, INotificationService notificationService, PaymentHandler paymentHandler)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
        this.notificationService = notificationService;
        this.paymentHandler = paymentHandler;
    }

    public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderResponse = await CreateOrderAsync(request.OrderRequest);

        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var user = await unitOfWork.UserRepository.GetById(userId);

        // Send Product Codes to the user
        if (user != null)
        {
            var productCodesInfo = await GenerateAndSendProductCodes(orderResponse, user.Email);

            notificationService.SendEmailToQueue(
                "Order Confirmation",
                user.Email,
                $"Your order {orderResponse.OrderNumber} has been confirmed. Total Price: {orderResponse.TotalPrice}, Total Reward Points: {orderResponse.TotalRewardAmount} Product Codes: {productCodesInfo}");
        }

        return new ApiResponse<OrderResponse>(orderResponse);
    }

    private async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);

        var cart = await unitOfWork.CartRepository.FirstOrDefaultAsync(c => c.UserId == userId, "CartProducts.Product");

        var wallet = await unitOfWork.WalletRepository.FirstOrDefaultAsync(w => w.UserId == userId);

        if (cart == null || !cart.CartProducts.Any())
        {
            throw new Exception("Cart is empty");
        }

        // Stock Control
        foreach (var cartProduct in cart.CartProducts)
        {
            var product = await unitOfWork.ProductRepository.GetById(cartProduct.ProductId);
            if (product == null || product.StockQuantity < cartProduct.Quantity)
            {
                throw new Exception($"Not enough stock: {product?.Name ?? "Unknown Product"}");
            }
        }

        // Calculate Price
        decimal totalPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price);
        decimal totalRewardPoints = cart.CartProducts.Sum(cp => CalculateRewardPoints(cp.Product, cp.Quantity));
        decimal finalPrice = totalPrice;

        //Check and Apply Coupon
        Coupon coupon = null;
        if (!string.IsNullOrEmpty(orderRequest.CouponCode))
        {
            coupon = await unitOfWork.CouponRepository.FirstOrDefaultAsync(c => c.Code == orderRequest.CouponCode);
            if (coupon != null)
            {

                if (coupon.CouponQuantity <= 0)
                {
                    throw new Exception("Coupon is out of stock.");
                }

                if (coupon.EndDate < DateTime.UtcNow)
                {
                    throw new Exception("Coupon is expired.");
                }

                finalPrice = ApplyDiscount(totalPrice, coupon.DiscountAmount);
                coupon.CouponQuantity -= 1;
                unitOfWork.CouponRepository.Update(coupon);
            }
        }

        decimal rewardPointsToApply = Math.Min(wallet.RewardPoints, finalPrice);
        finalPrice -= rewardPointsToApply;

        decimal balanceToApply = Math.Min(wallet.Balance, finalPrice);
        finalPrice -= balanceToApply;

        // Payment with credit card
        if (finalPrice > 0)
        {
            if (orderRequest.PaymentRequest == null)
            {
                throw new Exception("No payment information provided.");
            }

            // Payment transaction is completed
            var paymentSuccess = await paymentHandler.ProcessPaymentAsync(orderRequest.PaymentRequest, finalPrice);
            if (!paymentSuccess)
            {
                throw new Exception("Payment failed.");
            }
        }

        // Create Order and Order Details
        var order = new Order
        {
            UserId = userId,
            OrderNumber = GenerateUniqueOrderNumber(),
            Coupon = coupon,
            TotalPrice = totalPrice,
            FinalPrice = finalPrice,
            TotalRewardAmount = totalRewardPoints,
            OrderDetails = cart.CartProducts.Select(cp => new OrderDetail
            {
                ProductId = cp.ProductId,
                Quantity = cp.Quantity,
                UnitPrice = cp.Product.Price,
                RewardAmount = CalculateRewardPoints(cp.Product, cp.Quantity)
            }).ToList()
        };

        //Update Stock
        foreach (var cartProduct in cart.CartProducts)
        {
            var product = await unitOfWork.ProductRepository.GetById(cartProduct.ProductId);
            if (product != null)
            {
                product.StockQuantity -= cartProduct.Quantity;
                if (product.StockQuantity <= 0)
                {
                    product.IsActive = false;
                }
                unitOfWork.ProductRepository.Update(product);
            }
        }

        await unitOfWork.OrderRepository.Insert(order);
        cart.CartProducts.Clear();
        await unitOfWork.Complete();

        await LogWalletTransaction(wallet.Id, wallet.Balance, balanceToApply, "Balance used for order.");
        await LogWalletTransaction(wallet.Id, wallet.RewardPoints, rewardPointsToApply, "Reward points used for order.");
        await unitOfWork.Complete();

        wallet.Balance -= balanceToApply;
        wallet.RewardPoints -= rewardPointsToApply;
        wallet.RewardPoints += totalRewardPoints;
        unitOfWork.WalletRepository.Update(wallet);

        return new OrderResponse
        {
            OrderNumber = order.OrderNumber,
            TotalPrice = order.TotalPrice,
            FinalPrice = order.FinalPrice,
            OrderDetails = order.OrderDetails.Select(od => new OrderDetailResponse
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice,
                RewardAmount = od.RewardAmount
            }).ToList()
        };
    }

    // Send Product Codes to the user and delete the codes
    private async Task<string> GenerateAndSendProductCodes(OrderResponse orderResponse, string email)
    {
        var stringBuilder = new StringBuilder();

        foreach (var detail in orderResponse.OrderDetails)
        {
            var product = await unitOfWork.ProductRepository.GetById(detail.ProductId, "ProductCodes");
            var productCodes = product.ProductCodes.Take(detail.Quantity).ToList();

            stringBuilder.AppendLine($"Product: {product.Name}");
            foreach (var code in productCodes)
            {
                stringBuilder.AppendLine($"Code: {code.Code}");
            }

            // Delete the codes after adding them to the email
            foreach (var code in productCodes)
            {
                unitOfWork.ProductCodeRepository.Delete(code);
            }

            stringBuilder.AppendLine();
        }

        await unitOfWork.Complete(); // Save changes after deletion
        return stringBuilder.ToString();
    }

    private decimal CalculateRewardPoints(Product product, int quantity)
    {
        return Math.Min(product.Price * product.RewardPercantage / 100 * quantity, product.MaxRewardAmount);
    }

    private decimal ApplyDiscount(decimal totalPrice, decimal discountAmount)
    {
        return totalPrice - discountAmount;
    }

    private async Task LogWalletTransaction(long walletId, decimal initialAmount, decimal spentAmount, string description)
    {
        var walletTransaction = new WalletTransaction
        {
            WalletId = walletId,
            ReferenceNumber = GenerateUniqueOrderNumber(),
            InitialAmount = initialAmount,
            SpentAmount = spentAmount,
            RemainingAmount = initialAmount - spentAmount,
            Description = description,
            TransactionDate = DateTime.UtcNow
        };

        await unitOfWork.WalletTransactionRepository.Insert(walletTransaction);
    }

    private long GenerateUniqueOrderNumber()
    {
        return DateTime.UtcNow.Ticks / (long)Math.Pow(10, 9);
    }
}
