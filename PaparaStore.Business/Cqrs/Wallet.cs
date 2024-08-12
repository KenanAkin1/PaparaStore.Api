using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record CreateWalletCommand(WalletRequest Request) : IRequest<ApiResponse<WalletResponse>>;
public record UpdateWalletCommand(long WalletId, WalletRequest Request) : IRequest<ApiResponse>;
public record DeleteWalletCommand(long WalletId) : IRequest<ApiResponse>;

public record GetWalletByUserIdQuery() : IRequest<ApiResponse<WalletResponse>>;