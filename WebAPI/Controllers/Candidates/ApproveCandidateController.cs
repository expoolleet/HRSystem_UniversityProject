using Application.Candidates.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Candidates;

[ApiController]
[Route("api/[controller]")]
public class ApproveCandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApproveCandidateController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpPost("{candidateId:guid}")]
    public async Task<IActionResult> Approve([FromRoute] Guid candidateId, [FromBody] ApproveCandidateRequest? request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required");
        }

        var command = new ApproveCandidateCommand
        {
            CandidateId = candidateId,
            UserId = request.UserId,
            Feedback = request.Feedback,
        };

        await _mediator.Send(command);
        return Ok("Candidate approved successfully");
    }
}

public class ApproveCandidateRequest
{
    public Guid UserId { get; set; }
    public required string Feedback { get; set; }
}