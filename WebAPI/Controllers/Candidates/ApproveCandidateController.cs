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

    [HttpPost]
    public async Task<IActionResult> ApproveCandidate([FromBody] ApproveCandidateCommand? command)
    {
        if (command == null)
        {
            return BadRequest("Request body is required");
        }
        await _mediator.Send(command);
        return Ok("Candidate approved successfully");
    }
}