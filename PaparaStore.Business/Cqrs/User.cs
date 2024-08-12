using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Schema;

namespace PaparaStore.Business.Cqrs;
public record CreateUserCommand(UserRequest Request) : IRequest<ApiResponse<UserResponse>>;
public record CreateAdminUserCommand(UserRequest Request) : IRequest<ApiResponse<UserResponse>>;
public record UpdateUserCommand(UserRequest Request) : IRequest<ApiResponse>;
public record DeleteUserCommand() : IRequest<ApiResponse>;
public record DeleteUserForAdminCommand(long UserId) : IRequest<ApiResponse>;

public record GetAllUserQuery() : IRequest<ApiResponse<List<UserResponse>>>;
public record GetUserByIdQuery() : IRequest<ApiResponse<UserResponse>>;
public record GetUserByParameterQuery(string FirstName, string LastName, string Email) : IRequest<ApiResponse<List<UserResponse>>>;
