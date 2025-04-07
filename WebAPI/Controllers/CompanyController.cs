using Application.Companies.Models.Commands;
using Application.Companies.Models.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Dto.Companies;
using WebApi.Contracts.Requests.Companies;

namespace WebApi.Controllers;

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
    
    [HttpGet("users/{userId:guid}")]
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
    
    [HttpGet("users/{name}")]
    public async Task<IActionResult> GetUserByName(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByNameQuery
        {
            Name = name,
        };
        var result = await _mediator.Send(query, cancellationToken);
        var userDto = _mapper.Map<UserDto>(result);
        return Ok(userDto);
    }
    
    [HttpPost]
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
    
    [HttpPost("roles/{name}")]
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

    [HttpPost("{companyId:guid}/users")]
    public async Task<IActionResult> CreateUser(
        [FromRoute] Guid companyId,
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var command = new CreateUserCommand
        {
            RoleId = request.RoleId,
            CompanyId = companyId,
            Password = request.Password,
            Name = request.Name,
        };
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUserById), new {userId = result}, null);
    }
}