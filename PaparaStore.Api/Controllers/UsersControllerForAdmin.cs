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

    public class UsersControllerForAdmin : ControllerBase
    {
        private readonly IMediator mediator;

        public UsersControllerForAdmin(IMediator mediator)
        {
            this.mediator = mediator;

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<UserResponse>>> Get()
        {
            var operation = new GetAllUserQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("ByParameters")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<List<UserResponse>>> GetByParameters(
                    [FromQuery] string FirstName = null,
                    [FromQuery] string LastName = null,
                    [FromQuery] string Email = null)
        {
            var operation = new GetUserByParameterQuery(FirstName, LastName, Email);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost("create-admin")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<UserResponse>> CreateAdmin([FromBody] UserRequest value)
        {
            var operation = new CreateAdminUserCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{UserId}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(long UserId)
        {
            var operation = new DeleteUserForAdminCommand(UserId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}
