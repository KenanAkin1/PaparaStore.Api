using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

public class GetWalletTransactionsByReferenceNumberQueryHandler : IRequestHandler<GetWalletTransactionsByReferenceNumberQuery, ApiResponse<List<WalletTransactionResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetWalletTransactionsByReferenceNumberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<WalletTransactionResponse>>> Handle(GetWalletTransactionsByReferenceNumberQuery request, CancellationToken cancellationToken)
    {
        var walletTransactions = await unitOfWork.WalletTransactionRepository.Where(wt => wt.ReferenceNumber == request.ReferenceNumber);
        var mapped = mapper.Map<List<WalletTransactionResponse>>(walletTransactions);
        return new ApiResponse<List<WalletTransactionResponse>>(mapped);
    }
}
