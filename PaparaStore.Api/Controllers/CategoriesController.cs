using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Schema;

namespace PaparaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator mediator;


        // Yapıcı metodda IUnitOfWork enjeksiyonunu yapıyoruz
        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;

        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet]
        public async Task<ApiResponse<List<CategoryResponse>>> Get()
        {
            var operation = new GetAllCategoryQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet("{CategoryId}")]
        public async Task<ApiResponse<CategoryResponse>> Get([FromRoute] long CategoryId)
        {
            var operation = new GetCategoryByIdQuery(CategoryId);
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ApiResponse<CategoryResponse>> Post([FromBody] CategoryRequest value)
        {
            var operation = new CreateCategoryCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{CategoryId}")]
        public async Task<ApiResponse> Put(long CategoryId, [FromBody] CategoryRequest value)
        {
            var operation = new UpdateCategoryCommand(CategoryId, value);
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{CategoryId}")]
        public async Task<ApiResponse> Delete(long CategoryId)
        {
            var operation = new DeleteCategoryCommand(CategoryId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
