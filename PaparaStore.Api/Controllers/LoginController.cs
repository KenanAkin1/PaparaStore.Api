using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaparaStore.Base.Response;
using PaparaStore.Business.Cqrs;
using PaparaStore.Schema;
using PaparaStore.Base.Attribute;


namespace PaparaStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IMediator mediator;

    public LoginController(IMediator mediator)
    {
        this.mediator = mediator;
    }


    [HttpPost]
    [AllowAnonymous]
    [ResponseHeader("MyCustomHeaderInResponse", "POST")]
    public async Task<ApiResponse<AuthorizationResponse>> Post([FromBody] AuthorizationRequest value)
    {
        var operation = new CreateAuthorizationTokenCommand(value);
        var result = await mediator.Send(operation);
        return result;

    }

}