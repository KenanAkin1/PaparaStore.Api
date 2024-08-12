using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Security.Claims;


namespace PaparaStore.Business.Query;
public class GetWalletByUserIdQueryHandler : IRequestHandler<GetWalletByUserIdQuery, ApiResponse<WalletResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public GetWalletByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<ApiResponse<WalletResponse>> Handle(GetWalletByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var entity = await unitOfWork.WalletRepository.FirstOrDefaultAsync(w => w.UserId == userId, "User");
        var mapped = mapper.Map<WalletResponse>(entity);
        return new ApiResponse<WalletResponse>(mapped);
    }
}
