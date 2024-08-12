using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;

namespace PaparaStore.Business.Command;
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetById(request.CategoryId);

        if (category == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Category not found" };
        }

        mapper.Map(request.Request, category);

        unitOfWork.CategoryRepository.Update(category);
        await unitOfWork.Complete();

        await distributedCache.RemoveAsync("categoryList");
        return new ApiResponse { IsSuccess = true, Message = "Category updated successfully" };
    }
}
