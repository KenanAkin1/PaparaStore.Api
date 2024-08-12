using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PaparaStore.Business.Command
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
        }

        public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            if (request.Request.StockQuantity > request.Request.ProductCodes.Count)
            {
                return new ApiResponse<ProductResponse>("The number of product codes must match the quantity.");
            }
            var productEntity = mapper.Map<ProductRequest, Product>(request.Request);
            productEntity.IsActive = true;

            // Insert the Product entity
            await unitOfWork.ProductRepository.Insert(productEntity);
            await unitOfWork.Complete();

            foreach (var categoryId in request.Request.CategoryIds)
            {
                var existingCategoryProduct = await unitOfWork.CategoryProductRepository.FirstOrDefaultAsync(cp => cp.CategoryId == categoryId && cp.ProductId == productEntity.Id);

                if (existingCategoryProduct == null)
                {
                    var categoryProduct = new CategoryProduct
                    {
                        CategoryId = categoryId,
                        ProductId = productEntity.Id
                    };
                    await unitOfWork.CategoryProductRepository.Insert(categoryProduct);
                }
            }

            // Insert the ProductCodes manually
            foreach (var code in request.Request.ProductCodes)
            {
                var productCode = new ProductCode
                {
                    ProductId = productEntity.Id,
                    Code = code
                };
                await unitOfWork.ProductCodeRepository.Insert(productCode);
            }

            await unitOfWork.Complete();


            await distributedCache.RemoveAsync("productList");

            var productResponse = mapper.Map<ProductResponse>(productEntity);
            return new ApiResponse<ProductResponse>(productResponse);
        }

    }
}
