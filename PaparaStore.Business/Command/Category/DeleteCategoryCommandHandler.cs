using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;

namespace PaparaStore.Business.Command;
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryRepository.GetById(request.CategoryId, "CategoryProducts");

        if (category == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Category not found" };
        }

        if (category.CategoryProducts != null && category.CategoryProducts.Any())
        {
            return new ApiResponse { IsSuccess = false, Message = "Cannot delete category with associated products" };
        }

        await unitOfWork.CategoryRepository.Delete(request.CategoryId);
        await unitOfWork.Complete();

        await distributedCache.RemoveAsync("categoryList");
        return new ApiResponse { IsSuccess = true, Message = "Category deleted successfully" };
    }
}
