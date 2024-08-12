using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Security.Claims;


namespace PaparaStore.Business.Command;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResponse>

{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var existingUser = await unitOfWork.UserRepository.GetById(userId);

        mapper.Map(request.Request, existingUser, opts =>
        {
            opts.BeforeMap((src, dest) => {
                dest.Id = existingUser.Id; 
                //Role cant be changed
                dest.Role = existingUser.Role;
            });
        });

        unitOfWork.UserRepository.Update(existingUser);
        await unitOfWork.Complete();


        return new ApiResponse { IsSuccess = true, Message = "User updated successfully" }; 
    }
}
