using Application.Candidates.Models.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Candidates;

[ApiController]
[Route("api/[controller]")]
public class GetCandidatesByFilterController : ControllerBase
{
    private readonly IMediator _mediator;

    public GetCandidatesByFilterController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCandidatesByFilter([FromBody] GetCandidatesByFilterQuery? query)
    {
        if (query == null)
        {
            return BadRequest("Request body is required");
        }
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }
}