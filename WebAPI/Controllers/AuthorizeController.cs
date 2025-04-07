using Application.Companies.Models.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Requests.Companies;
using WebApi.Contracts.Responses.Companies;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthorizeController(IMediator mediator, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(mapper);
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AuthorizeUser(
        [FromBody] AuthUserRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var command = new AuthorizeUserCommand
        {
            Login = request.Login,
            Password = request.Password
        };
        var result = await _mediator.Send(command, cancellationToken);
        var response = _mapper.Map<AuthUserResponse>(result);
        return Ok(response);
    }
}