using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record AddWalletBalanceCommand : IRequest<ApiResponse>
{
    public decimal Amount { get; set; }
    public PaymentRequest PaymentRequest { get; set; }

    public AddWalletBalanceCommand(decimal amount, PaymentRequest paymentRequest)
    {
        Amount = amount;
        PaymentRequest = paymentRequest;
    }
}

public record GetWalletByUserIdQuery() : IRequest<ApiResponse<WalletResponse>>;