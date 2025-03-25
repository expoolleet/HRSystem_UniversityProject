using Application.Candidates.Models.Queries;
using Domain.Candidates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Candidates;

[ApiController]
[Route("api/[controller]")]
public class GetCandidateController : Controller
{
    private readonly IMediator _mediator;

    public GetCandidateController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpGet("{candidateId:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid candidateId)
    {
        var query = new GetCandidateQuery
        {
            CandidateId = candidateId
        };
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}