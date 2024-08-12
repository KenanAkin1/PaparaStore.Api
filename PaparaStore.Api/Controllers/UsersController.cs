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
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;

        }

        [HttpGet("User")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse<UserResponse>> GetById()
        {
            var operation = new GetUserByIdQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse<UserResponse>> Post([FromBody] UserRequest value)
        {
            var operation = new CreateUserCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }


        [HttpPut("User")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse> Put([FromBody] UserRequest value)
        {
            var operation = new UpdateUserCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("User")]
        [Authorize(Roles = "customer,admin")]
        public async Task<ApiResponse> Delete()
        {
            var operation = new DeleteUserCommand();
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
