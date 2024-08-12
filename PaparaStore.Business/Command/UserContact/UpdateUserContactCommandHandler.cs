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
public class UpdateUserContactCommandHandler : IRequestHandler<UpdateUserContactCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UpdateUserContactCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Handle(UpdateUserContactCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);

        var existingUserContact = await unitOfWork.UserContactRepository.FirstOrDefaultAsync(uc => uc.UserId == userId);

        if (existingUserContact == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "UserContact not found" };
        }

        mapper.Map(request.Request, existingUserContact);

        unitOfWork.UserContactRepository.Update(existingUserContact);
        await unitOfWork.Complete();
        return new ApiResponse { IsSuccess = true, Message = "UserContact updated successfully" };
    }
}
