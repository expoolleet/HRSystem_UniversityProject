using Application.Candidates.Models.Commands;
using Application.Candidates.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Requests;
using WebApi.Contracts.Requests.Candidates;
using WebApi.Contracts.Responses;
using WebApi.Contracts.Responses.Candidates;

namespace WebApi.Controllers.Candidates;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }
    
    [HttpPost("approve/{candidateId:guid}")]
    public async Task<IActionResult> ApproveCandidate(
        [FromRoute] Guid candidateId,
        [FromBody] ApproveCandidateRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
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
    
    [HttpPost("reject/{candidateId:guid}")]
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
        [FromQuery] GetCandidatesByFilterRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var query = new GetCandidatesByFilterRequest
        {
            CompanyId = request.CompanyId,
            Title = request.Title,
            Page = request.Page,
            PageSize = request.PageSize,
        };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}