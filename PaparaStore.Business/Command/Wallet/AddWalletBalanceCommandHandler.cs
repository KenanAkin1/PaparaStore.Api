using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Command.Order;
using PaparaStore.Business.Cqrs;
using PaparaStore.Business.Notification;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Business.Command;
internal class AddWalletBalanceCommandHandler : IRequestHandler<AddWalletBalanceCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly INotificationService notificationService;
    private readonly PaymentHandler paymentHandler;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AddWalletBalanceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService, PaymentHandler paymentHandler, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.notificationService = notificationService;
        this.paymentHandler = paymentHandler;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Handle(AddWalletBalanceCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var wallet = await unitOfWork.WalletRepository.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Wallet not found" };
        }

        // Payment
        var paymentSuccess = await paymentHandler.ProcessPaymentAsync(request.PaymentRequest, request.Amount);
        if (!paymentSuccess)
        {
            return new ApiResponse { IsSuccess = false, Message = "Payment failed" };
        }

        var initialAmount = wallet.Balance;
        wallet.Balance += request.Amount;
        unitOfWork.WalletRepository.Update(wallet);

        // Log the wallet transaction
        var walletTransaction = new WalletTransaction
        {
            WalletId = wallet.Id,
            ReferenceNumber = GenerateUniqueTransactionNumber(),
            InitialAmount = initialAmount,
            SpentAmount = 0,
            RemainingAmount = wallet.Balance,
            Description = "Balance added via payment",
            TransactionDate = DateTime.UtcNow
        };

        await unitOfWork.WalletTransactionRepository.Insert(walletTransaction);
        await unitOfWork.Complete();

        return new ApiResponse { IsSuccess = true, Message = "Balance added successfully" };
    }
    private long GenerateUniqueTransactionNumber()
    {
        return DateTime.UtcNow.Ticks / (long)Math.Pow(10, 9);
    }
}