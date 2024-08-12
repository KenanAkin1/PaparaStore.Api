using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Text;

namespace PaparaStore.Business.Query
{
    public class GetAllProductByCategoryQueryHandler : IRequestHandler<GetAllProductByCategoryQuery, ApiResponse<List<ProductResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;

        public GetAllProductByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.distributedCache = distributedCache;
        }

        public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "productList";
            var cachedData = await distributedCache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                string json = Encoding.UTF8.GetString(cachedData);
                var responseObj = JsonConvert.DeserializeObject<List<ProductResponse>>(json);
                return new ApiResponse<List<ProductResponse>>(responseObj);
            }

            var products = await unitOfWork.ProductRepository.Where(
                p => p.CategoryProducts.Any(cp => cp.Category.Name.Equals(request.CategoryName, StringComparison.OrdinalIgnoreCase)) && p.IsActive,
                "CategoryProducts.Category"
            );

            var mappedList = mapper.Map<List<ProductResponse>>(products);

            if (mappedList.Any())
            {
                string responseStr = JsonConvert.SerializeObject(mappedList);
                byte[] responseArr = Encoding.UTF8.GetBytes(responseStr);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
                await distributedCache.SetAsync(cacheKey, responseArr, cacheOptions);
            }

            return new ApiResponse<List<ProductResponse>>(mappedList);
        }
    }
}
