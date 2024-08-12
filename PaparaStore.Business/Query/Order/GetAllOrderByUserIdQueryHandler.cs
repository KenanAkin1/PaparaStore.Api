using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Security.Claims;


namespace PaparaStore.Business.Query;
public class GetAllOrderByUserIdQueryHandler : IRequestHandler<GetAllOrderByUserIdQuery, ApiResponse<List<OrderResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public GetAllOrderByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<ApiResponse<List<OrderResponse>>> Handle(GetAllOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        List<Order> entityList = await unitOfWork.OrderRepository.Where(o => o.UserId == userId, "OrderDetails.Product", "User");
        var mappedList = mapper.Map<List<OrderResponse>>(entityList);
        return new ApiResponse<List<OrderResponse>>(mappedList);
    }
}
