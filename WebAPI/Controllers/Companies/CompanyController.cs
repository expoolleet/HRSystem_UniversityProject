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
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CompanyController(IMediator mediator, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(mapper);
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("user/create")]
    public async Task<IActionResult> CreateUser(
        [FromBody] AddUserRequest request,
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

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid userId,
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
    
    [HttpGet("user")]
    public async Task<IActionResult> GetUserByLogin(
        [FromQuery] string login,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByLoginQuery
        {
            Login = login,
        };
        var result = await _mediator.Send(query, cancellationToken);
        var userDto = _mapper.Map<UserDto>(result);
        return Ok(userDto);
    }
    
    [AllowAnonymous]
    [HttpPost("authorize")]
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
        var response = new AuthUserResponse
        {
            UserId = result.UserId,
            Role = _mapper.Map<RoleDto>(result.Role),
            Token = _mapper.Map<TokenDto>(result.Token)
        };
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCompany(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var command = new CreateCompanyCommand
        {
            Name = name,
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("role/create")]
    public async Task<IActionResult> CreateRole(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var command = new CreateRoleCommand
        {
            Name = name,
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}