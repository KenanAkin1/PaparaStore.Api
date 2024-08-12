using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Query;
public class GetOrderByOrderNumberQueryHandler : IRequestHandler<GetOrderByOrderNumberQuery, ApiResponse<OrderResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetOrderByOrderNumberQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<OrderResponse>> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.OrderRepository.FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber, "OrderDetails.Product");

        if (entity == null)
        {
            return new ApiResponse<OrderResponse>("Order not found");
        }
        var mapped = mapper.Map<OrderResponse>(entity);
        return new ApiResponse<OrderResponse>(mapped);
    }
}