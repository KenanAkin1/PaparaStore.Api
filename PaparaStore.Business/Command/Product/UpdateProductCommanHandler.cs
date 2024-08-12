using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;

namespace PaparaStore.Business.Command;
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;


    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository.GetById(request.ProductId);

        if (product == null)
        {
            return new ApiResponse { IsSuccess = false, Message = "Product not found" };
        }

        foreach (var categoryId in request.Request.CategoryIds)
        {
            var existingCategoryProduct = await unitOfWork.CategoryProductRepository.FirstOrDefaultAsync(cp => cp.CategoryId == categoryId && cp.ProductId == product.Id);

            if (existingCategoryProduct != null)
            {

                unitOfWork.CategoryProductRepository.Update(existingCategoryProduct);
            }
        }

        //Make last price for discount system
        product.LastPrice = product.Price;

        mapper.Map(request.Request, product);
        unitOfWork.ProductRepository.Update(product);
        await unitOfWork.Complete();

        await distributedCache.RemoveAsync("productList");
        return new ApiResponse { IsSuccess = true, Message = "Product updated successfully" }; ;
    }
}
