using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record AddProductToCartCommand(long ProductId, int Quantity) : IRequest<ApiResponse>;
public record RemoveProductFromCartCommand(long ProductId) : IRequest<ApiResponse>;



public record GetCartByUserIdQuery() : IRequest<ApiResponse<CartResponse>>;