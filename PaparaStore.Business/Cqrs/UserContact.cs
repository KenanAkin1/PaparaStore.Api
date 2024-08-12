using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record CreateUserContactCommand(UserContactRequest Request) : IRequest<ApiResponse<UserContactResponse>>;
public record UpdateUserContactCommand(UserContactRequest Request) : IRequest<ApiResponse>;
public record DeleteUserContactCommand() : IRequest<ApiResponse>;

public record GetUserContactByUserIdQuery() : IRequest<ApiResponse<UserContactResponse>>;

