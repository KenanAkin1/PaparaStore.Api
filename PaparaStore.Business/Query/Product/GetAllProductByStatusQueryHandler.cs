using AutoMapper;
using MediatR;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Data.Domain;
using PaparaStore.Data.UnitOfWork;
using PaparaStore.Schema;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace PaparaStore.Business.Query;

public class GetAllProductByStatusQueryHandler : IRequestHandler<GetAllProductByStatusQuery, ApiResponse<List<ProductResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;

    public GetAllProductByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductByStatusQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"productList_{request.IsActive}";
        var cachedData = await distributedCache.GetAsync(cacheKey);

        if (cachedData != null)
        {
            string json = Encoding.UTF8.GetString(cachedData);
            var responseObj = JsonConvert.DeserializeObject<List<ProductResponse>>(json);
            return new ApiResponse<List<ProductResponse>>(responseObj);
        }

        List<Product> entityList = await unitOfWork.ProductRepository.Where(p => p.IsActive == request.IsActive, "CategoryProducts.Category");
        var mappedList = mapper.Map<List<ProductResponse>>(entityList);

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
