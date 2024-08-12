using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using System.Text;

namespace PaparaStore.Business.Query
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ApiResponse<List<ProductResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;

        public GetAllProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _distributedCache = distributedCache;
        }

        public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "productList";
            var cachedData = await _distributedCache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                string json = Encoding.UTF8.GetString(cachedData);
                var responseObj = JsonConvert.DeserializeObject<List<ProductResponse>>(json);
                return new ApiResponse<List<ProductResponse>>(responseObj);
            }

            List<Product> entityList = await _unitOfWork.ProductRepository.GetAll("CategoryProducts.Category");
            var activeProducts = entityList.Where(p => p.IsActive).ToList();
            var mappedList = _mapper.Map<List<ProductResponse>>(activeProducts);
            var response = new ApiResponse<List<ProductResponse>>(mappedList);

            if (mappedList.Any())
            {
                string responseStr = JsonConvert.SerializeObject(response.Data);
                byte[] responseArr = Encoding.UTF8.GetBytes(responseStr);
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
                await _distributedCache.SetAsync(cacheKey, responseArr, cacheOptions);
            }

            return response;
        }
    }
}
