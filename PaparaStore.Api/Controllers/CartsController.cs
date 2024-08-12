using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;


namespace PaparaStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator mediator;

        public CartController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet("cart")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var query = new GetCartByUserIdQuery();
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "customer,admin")]
        [HttpDelete("RemoveFromCart")]
        public async Task<ApiResponse> RemoveFromCart([FromBody] RemoveProductFromCartCommand command)
        {
            var result = await mediator.Send(command);
            return result;
        }
    }
}
