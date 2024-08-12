using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;


namespace PaparaStore.Business.Query;
public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApiResponse<List<UserResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetAllUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<UserResponse>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        List<User> entityList = await unitOfWork.UserRepository.GetAll("UserContacts", "Wallet");
        var mappedList = mapper.Map<List<UserResponse>>(entityList);
        return new ApiResponse<List<UserResponse>>(mappedList);
    }
}
