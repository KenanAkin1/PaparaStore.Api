using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Business.Command;
public class CreateUserContactCommandHandler : IRequestHandler<CreateUserContactCommand, ApiResponse<UserContactResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CreateUserContactCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse<UserContactResponse>> Handle(CreateUserContactCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = long.Parse(userIdClaim.Value);
        var mapped = mapper.Map<UserContactRequest, UserContact>(request.Request);
        mapped.UserId = userId;
        await unitOfWork.UserContactRepository.Insert(mapped);
        await unitOfWork.Complete();
        var response = mapper.Map<UserContactResponse>(mapped);
        return new ApiResponse<UserContactResponse>(response);
    }
}
