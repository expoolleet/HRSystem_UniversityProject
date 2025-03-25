using Application.Companies.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Companies;

[ApiController]
[Route("api/[controller]")]
public class AuthorizeUserController : ControllerBase
{
    private readonly Mediator _mediator;

    public AuthorizeUserController(Mediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AuthorizeUser([FromBody] AuthorizeUserCommand? command)
    {
        if (command == null)
        {
            return BadRequest("Request body is required");
        }
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}