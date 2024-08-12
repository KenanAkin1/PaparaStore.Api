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
    public class UserContactsController : ControllerBase
    {
        private readonly IMediator mediator;


        // Yapıcı metodda IUnitOfWork enjeksiyonunu yapıyoruz
        public UserContactsController(IMediator mediator)
        {
            this.mediator = mediator;

        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet("ByUserId")]
        public async Task<ApiResponse<UserContactResponse>> Get()
        {
            var operation = new GetUserContactByUserIdQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpPost]
        public async Task<ApiResponse<UserContactResponse>> Post([FromBody] UserContactRequest value)
        {
            var operation = new CreateUserContactCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpPut("ByUserId")]
        public async Task<ApiResponse> Put( [FromBody] UserContactRequest value)
        {
            var operation = new UpdateUserContactCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }

        [Authorize(Roles = "customer,admin")]
        [HttpDelete("ByUserId")]
        public async Task<ApiResponse> Delete()
        {
            var operation = new DeleteUserContactCommand();
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
