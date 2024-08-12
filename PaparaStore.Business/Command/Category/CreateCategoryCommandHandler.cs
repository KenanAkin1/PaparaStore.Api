using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse<CategoryResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var mapped = mapper.Map<Category>(request.Request);
        await unitOfWork.CategoryRepository.Insert(mapped);
        await unitOfWork.Complete();

        var response = mapper.Map<CategoryResponse>(mapped);
        await distributedCache.RemoveAsync("categoryList");

        return new ApiResponse<CategoryResponse>(response);
    }
}
