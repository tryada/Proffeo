using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proffeo.Api.Users;

[Authorize, ApiController, Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = UserContractsExtensions.GetUsersQuery(page, pageSize);
        var queryResult = await mediator.Send(query);
        return Ok(queryResult.ToResponse());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = UserContractsExtensions.GetUserQuery(id);
        var queryResult = await mediator.Send(query);
        return Ok(queryResult.ToResponse());
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserRequest request)
    {
        var command = request.ToCommand();
        var commandResult = await mediator.Send(command);
        return Ok(commandResult.ToResponse());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, UpdateUserRequest request)
    {
        var command = request.ToCommand(id);
        var commandResult = await mediator.Send(command);
        return Ok(commandResult.ToResponse());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = UserContractsExtensions.DeleteUserCommand(id);
        await mediator.Send(command);
        return Ok();
    }
}
