using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Query;
public class GetCategoryByCategoryNameQueryHandler : IRequestHandler<GetCategoryByCategoryNameQuery, ApiResponse<List<CategoryResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCategoryByCategoryNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<List<CategoryResponse>>> Handle(GetCategoryByCategoryNameQuery request, CancellationToken cancellationToken)
    {
        var entityList = await unitOfWork.CategoryRepository.Where(x => x.Name == request.Name);


        var mappedList = mapper.Map<List<CategoryResponse>>(entityList);
        return new ApiResponse<List<CategoryResponse>>(mappedList);
    }
}