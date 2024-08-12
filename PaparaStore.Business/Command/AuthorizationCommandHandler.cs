using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Business.Token;
using PaparaStore.Data.Service;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Command;
public class AuthorizationCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, ApiResponse<AuthorizationResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ITokenService tokenService;
    private readonly IHashingService hashingService;

    public AuthorizationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IHashingService hashingService)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.tokenService = tokenService;
        this.hashingService = hashingService;
    }

    public async Task<ApiResponse<AuthorizationResponse>> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == request.Request.Email);
        if (user is null)
            return new ApiResponse<AuthorizationResponse>("Invalid user information. Check your username or password. E1");

        var hashedPassword = hashingService.HashPassword(request.Request.Password);
        if (user.Password != hashedPassword)
        {
            return new ApiResponse<AuthorizationResponse>("Invalid user information. Check your username or password. E1");
        }

        if (user.Status != 1)
            return new ApiResponse<AuthorizationResponse>("Invalid user information. Check your username or password. E2");

        user.LastLoginDate = DateTime.Now;
        unitOfWork.UserRepository.Update(user);
        await unitOfWork.Complete();

        var token = await tokenService.GetToken(user);
        AuthorizationResponse response = new AuthorizationResponse()
        {
            ExpireTime = DateTime.Now.AddMinutes(5),
            AccessToken = token,
            Email = user.Email
        };

        return new ApiResponse<AuthorizationResponse>(response);
    }
}
