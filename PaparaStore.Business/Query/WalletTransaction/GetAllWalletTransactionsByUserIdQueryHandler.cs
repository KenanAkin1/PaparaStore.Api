using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Security.Claims;

public class GetAllWalletTransactionsByUserIdQueryHandler : IRequestHandler<GetAllWalletTransactionsByUserIdQuery, ApiResponse<List<WalletTransactionResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public GetAllWalletTransactionsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<List<WalletTransactionResponse>>> Handle(GetAllWalletTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var walletTransactions = await unitOfWork.WalletTransactionRepository.Where(wt => wt.Wallet.UserId == userId);
        var mapped = mapper.Map<List<WalletTransactionResponse>>(walletTransactions);
        return new ApiResponse<List<WalletTransactionResponse>>(mapped);
    }
}
