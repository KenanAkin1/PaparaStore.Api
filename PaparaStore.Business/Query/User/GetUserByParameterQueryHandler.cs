using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Query;
public class GetUserByParameterQueryHandler : IRequestHandler<GetUserByParameterQuery, ApiResponse<List<UserResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetUserByParameterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<UserResponse>>> Handle(GetUserByParameterQuery request, CancellationToken cancellationToken)
    {
        /*var predicate = PredicateBuilder.New<User>(true);
        if (!string.IsNullOrEmpty(request.UserNumber))
            predicate.And(x => x.UserNumber == request.UserNumber);
        if (!string.IsNullOrEmpty(request.FirstName))
            predicate.And(x => x.FirstName == request.FirstName);
        if (!string.IsNullOrEmpty(request.LastName))
            predicate.And(x => x.LastName == request.LastName);
        */

        var entityList = await unitOfWork.UserRepository.Where(
            x =>
            (x.FirstName == request.FirstName && x.LastName == request.LastName) ||
            x.Email == request.Email, "UserContacts", "Wallet");


        var mappedList = mapper.Map<List<UserResponse>>(entityList);
        return new ApiResponse<List<UserResponse>>(mappedList);
    }
}