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
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator _mediator)
        {
            this._mediator = _mediator;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet]
        public async Task<ApiResponse<List<ProductResponse>>> Get()
        {
            var operation = new GetAllProductQuery();
            var result = await _mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet("{productId}")]
        public async Task<ApiResponse<ProductResponse>> Get([FromRoute] long productId)
        {
            var operation = new GetProductByIdQuery(productId);
            var result = await _mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet("GetByCategory")]
        public async Task<ApiResponse<List<ProductResponse>>> GetByCategory(string categoryName)
        {
            var query = new GetAllProductByCategoryQuery(categoryName);
            var result = await _mediator.Send(query);
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ApiResponse<ProductResponse>> Post([FromBody] ProductRequest value)
        {
            var operation = new CreateProductCommand(value);
            var result = await _mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpPost("AddToCart")]
        public async Task<ApiResponse> AddToCart([FromBody] AddProductToCartCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{productId}")]
        public async Task<ApiResponse> Put([FromRoute] long productId, [FromBody] ProductRequest value)
        {
            var operation = new UpdateProductCommand(productId, value);
            var result = await _mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{productId}")]
        public async Task<ApiResponse> Delete([FromRoute] long productId)
        {
            var operation = new DeleteProductCommand(productId);
            var result = await _mediator.Send(operation);
            return result;
        }
    }
}
