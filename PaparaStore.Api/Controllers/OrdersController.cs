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
    public class OrdersController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet]
        public async Task<ApiResponse<List<OrderResponse>>> Get()
        {
            var operation = new GetAllOrderByUserIdQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet("{OrderNumber}")]
        public async Task<ApiResponse<OrderResponse>> Get([FromRoute] long OrderNumber)
        {
            var operation = new GetOrderByOrderNumberQuery(OrderNumber);
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpPost]
        public async Task<ApiResponse<OrderResponse>> Post([FromBody] OrderRequest orderRequest)
        {
            var operation = new CreateOrderCommand(orderRequest);
            var result = await mediator.Send(operation);
            return result;
        }
        
    }
}
