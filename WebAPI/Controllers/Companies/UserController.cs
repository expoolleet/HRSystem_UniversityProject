using Application.Companies.Models.Commands;
using Application.Companies.Models.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Dto.Companies;
using WebApi.Contracts.Requests.Companies;
using WebApi.Contracts.Responses.Companies;

namespace WebApi.Controllers.Companies;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(mapper);
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(
        [FromQuery] AddUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var command = new CreateUserCommand
        {
            RoleId = request.RoleId,
            CompanyId = request.CompanyId,
            Password = request.Password,
            Name = request.Name,
        };
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new {id = result}, null);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery
        {
            Id = userId,
        };
        var result = await _mediator.Send(query, cancellationToken);
        var userDto = _mapper.Map<UserDto>(result);
        return Ok(userDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> AuthorizeUser(
        [FromBody] AuthUserRequest request, 
        CancellationToken cancellationToken)
    {
        var command = new AuthorizeUserCommand
        {
            Login = request.Login,
            Password = request.Password
        };
        var result = await _mediator.Send(command, cancellationToken);
        var response = new AuthUserResponse
        {
            UserId = result.UserId,
            Role = _mapper.Map<RoleDto>(result.Role),
            Token = _mapper.Map<TokenDto>(result.Token)
        };
        return Ok(response);
    }
}