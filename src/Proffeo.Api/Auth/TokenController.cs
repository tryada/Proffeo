using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proffeo.Api.Auth;

[AllowAnonymous, ApiController, Route("api/[controller]") ]
public class TokenController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var command = TokenContractsExtensions.CreateTokenCommand();
        var result = await mediator.Send(command);
        return Ok(result.ToResponse());
    }
}