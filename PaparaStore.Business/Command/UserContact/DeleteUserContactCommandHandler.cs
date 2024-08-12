using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using System.Security.Claims;

namespace PaparaStore.Business.Command;
public class DeleteUserContactCommandHandler : IRequestHandler<DeleteUserContactCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public DeleteUserContactCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Handle(DeleteUserContactCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var userContact = await unitOfWork.UserContactRepository.FirstOrDefaultAsync(uc => uc.UserId == userId);
        if (userContact == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "UserContact not found" };
        }
        await unitOfWork.UserContactRepository.Delete(userContact.Id);
        await unitOfWork.Complete();

        return new ApiResponse { IsSuccess = true, Message = "UserContact deleted successfully" };
    }
}