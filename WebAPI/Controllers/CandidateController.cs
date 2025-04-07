using System.Security.Claims;
using Application.Candidates.Models.Commands;
using Application.Candidates.Models.Queries;
using Application.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Requests;
using WebApi.Contracts.Requests.Candidates;
using WebApi.Contracts.Responses.Candidates;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CandidateController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRoleContext _roleContext;

    public CandidateController(IMediator mediator, IRoleContext roleContext)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(roleContext);
        _mediator = mediator;
        _roleContext = roleContext;
    }
    
        
    [HttpGet("{candidateId:guid}")]
    public async Task<IActionResult> GetCandidate(
        [FromRoute] Guid candidateId, 
        CancellationToken cancellationToken)
    {
        var query = new GetCandidateQuery
        {
            CandidateId = candidateId
        };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCandidatesByFilter(
        [FromForm] GetCandidatesByFilterRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var query = new GetCandidatesByFilterQuery
        {
            CompanyId = request.CompanyId,
            Title = request.Title,
            Page = request.Page,
            PageSize = request.PageSize,
        };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("{candidateId:guid}/approve")]
    public async Task<IActionResult> ApproveCandidate(
        [FromRoute] Guid candidateId,
        [FromBody] ApproveCandidateRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var role = _roleContext.GetUserRoleName(request.UserId, cancellationToken);
        var command = new ApproveCandidateCommand
        {
            CandidateId = candidateId,
            UserId = request.UserId,
            Feedback = request.Feedback,
        };
        await _mediator.Send(command, cancellationToken);
        var response = new ApproveCandidateResponse
        {
            CandidateId = candidateId,
        };
        return Ok(response);
    }
    
    [HttpPost("{candidateId:guid}/reject")]
    public async Task<IActionResult> RejectCandidate(
        [FromRoute] Guid candidateId,
        [FromBody] RejectCandidateRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var command = new RejectCandidateCommand
        {
            CandidateId = candidateId,
            UserId = request.UserId,
            Feedback = request.Feedback,
        };
        await _mediator.Send(command, cancellationToken);
        var response = new RejectCandidateResponse
        {
            CandidateId = candidateId,
        };
        return Ok(response);
    }
}