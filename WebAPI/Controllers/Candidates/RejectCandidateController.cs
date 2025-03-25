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

    [HttpPost]
    public async Task<IActionResult> RejectCandidate([FromBody] RejectCandidateCommand? command)
    {
        if (command == null)
        {
            return BadRequest("Request body is required");
        }
        await _mediator.Send(command);
        return Ok("Candidate rejected successfully");
    }
}   