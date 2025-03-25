using Application.Companies.Models.Commands;
using Domain.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Companies;

[ApiController]
[Route("api/[controller]")]
public class AddUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddUserController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User? user)
    {
        if (user == null)
        {
            return BadRequest("Request body is required");
        }
        await _mediator.Send(new AddUserCommand { User = user });
        return Ok(user.Id);
    }
}