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

namespace PaparaStore.Business.Query;
public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, ApiResponse<List<CategoryResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly IDistributedCache distributedCache;

    public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache distributedCache)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.distributedCache = distributedCache;
    }

    public async Task<ApiResponse<List<CategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = "categoryList";
        var cachedData = await distributedCache.GetAsync(cacheKey);

        if (cachedData != null)
        {
            string json = Encoding.UTF8.GetString(cachedData);
            var responseObj = JsonConvert.DeserializeObject<List<CategoryResponse>>(json);
            return new ApiResponse<List<CategoryResponse>>(responseObj);
        }

        List<Category> entityList = await unitOfWork.CategoryRepository.GetAll();
        var mappedList = mapper.Map<List<CategoryResponse>>(entityList);

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

        return new ApiResponse<List<CategoryResponse>>(mappedList);
    }
}
