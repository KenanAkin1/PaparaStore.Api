using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Query;
public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse<CategoryResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    public async Task<ApiResponse<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.CategoryRepository.GetById(request.CategoryId);
        var mapped = mapper.Map<CategoryResponse>(entity);
        return new ApiResponse<CategoryResponse>(mapped);
    }
}