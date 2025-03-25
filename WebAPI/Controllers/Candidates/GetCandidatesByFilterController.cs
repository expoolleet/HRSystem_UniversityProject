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
    public async Task<IActionResult> Get([FromBody] GetCandidatesByFilterRequest? request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required");
        }
        
        var result = await _mediator.Send(request);
        
        return Ok(result);
    }
}

public class GetCandidatesByFilterRequest
{
    public Guid? CompanyId { get; set; }
    public string? Title { get; set; }
    public int Pages { get; set; }
    public int PageSize { get; set; }
}