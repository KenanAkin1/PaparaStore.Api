using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;

public record CreateOrderCommand(OrderRequest OrderRequest) : IRequest<ApiResponse<OrderResponse>>;

public record UpdateOrderCommand(long OrderId, OrderRequest Request) : IRequest<ApiResponse>;
public record DeleteOrderCommand(long OrderId) : IRequest<ApiResponse>;

public record GetAllOrderByUserIdQuery() : IRequest<ApiResponse<List<OrderResponse>>>;
public record GetOrderByOrderNumberQuery(long OrderNumber) : IRequest<ApiResponse<OrderResponse>>;
