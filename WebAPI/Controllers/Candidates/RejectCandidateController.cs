using Application.Candidates.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Candidates;

[ApiController]
[Route("api/[controller]")]
public class RejectCandidateController : ControllerBase
{
    private readonly IMediator _mediator;

    public RejectCandidateController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpPost("{candidateId:guid}")]
    public async Task<IActionResult> Reject([FromRoute] Guid candidateId, [FromBody] RejectCandidateRequest? request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required");
        }

        var command = new RejectCandidateCommand
        {
            CandidateId = candidateId,
            UserId = request.UserId,
            Feedback = request.Feedback,
        };

        await _mediator.Send(command);
        return Ok("Candidate rejected successfully");
    }
}   

public class RejectCandidateRequest
{
    public Guid UserId { get; set; }
    public string Feedback { get; set; } = string.Empty;
}