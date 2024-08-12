using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using System.Security.Claims;

namespace PaparaStore.Business.Command;

public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AddProductToCartCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);

        var user = await unitOfWork.UserRepository.GetById(userId, "Cart.CartProducts.Product");

        if (user == null)
        {
            return new ApiResponse("User not found");
        }

        var product = await unitOfWork.ProductRepository.GetById(request.ProductId);

        if (product == null || product.StockQuantity < request.Quantity)
        {
            return new ApiResponse("Product not found or insufficient stock");
        }

        //İf Cart not exists, create it
        var cart = user.Cart ?? new Cart
        {
            UserId = userId,
            IsActive = true,
            CartProducts = new List<CartProduct>()
        };

        var cartProduct = cart.CartProducts?.FirstOrDefault(cp => cp.ProductId == request.ProductId);

        // Update the cart product
        if (cartProduct != null)
        {
            cartProduct.Quantity += request.Quantity;
            cartProduct.RewardAmount = CalculateRewardAmount(product, cartProduct.Quantity);
        }

        //Add new cart product
        else
        {
            var newCartProduct = new CartProduct
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                RewardAmount = CalculateRewardAmount(product, request.Quantity),
                Product = product // Ensure the product is assigned here
            };
            cart.CartProducts.Add(newCartProduct);
        }

        // Recalculate TotalPrice and TotalRewardPoints for the entire cart
        cart.TotalPrice = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price);
        cart.ProductAmount = cart.CartProducts.Sum(cp => cp.Quantity);
        cart.TotalRewardPoints = cart.CartProducts.Sum(cp => cp.RewardAmount);

        if (user.Cart == null)
        {
            await unitOfWork.CartRepository.Insert(cart);
        }
        else
        {
            unitOfWork.CartRepository.Update(cart);
        }

        await unitOfWork.Complete();

        return new ApiResponse("Product added to cart successfully");
    }



    private decimal CalculateRewardAmount(Product product, int quantity)
    {
        var totalPotentialReward = product.Price * product.RewardPercantage / 100 * quantity;

        var maxRewardPoints = product.MaxRewardAmount;

        return Math.Min(totalPotentialReward, maxRewardPoints);
    }
}
