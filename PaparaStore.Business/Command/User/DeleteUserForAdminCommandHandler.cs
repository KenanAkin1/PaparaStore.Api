using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaparaStore.Business.Command;
public class DeleteUserForAdminCommandHandler : IRequestHandler<DeleteUserForAdminCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public DeleteUserForAdminCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse> Handle(DeleteUserForAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetById(request.UserId);

        if (user == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "User not found" };
        }
        await unitOfWork.UserRepository.Delete(request.UserId);
        await unitOfWork.Complete();
        return new ApiResponse { IsSuccess = true, Message = "User deleted successfully" };
    }
}