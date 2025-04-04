using Application.Vacancies.Models.Commands;
using Application.Vacancies.Models.Queries;
using AutoMapper;
using Domain.Candidates;
using Domain.Vacancies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts.Dto.Candidates;
using WebApi.Contracts.Dto.Vacancies;
using WebApi.Contracts.Requests.Vacancies;

namespace WebApi.Controllers.Vacancies;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VacancyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public VacancyController(IMediator mediator, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(mapper);
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateVacancy(
        [FromForm] CreateVacancyRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var command = new CreateVacancyCommand
        {
            CompanyId = request.CompanyId,
            Description = request.Description,
            Workflow = _mapper.Map<VacancyWorkflow>(request.Workflow)
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
        //return CreatedAtAction(nameof(CreateVacancy), new { id = result }, null);
    }
    
    [HttpPut("edit")]
    public async Task<IActionResult> EditVacancy(
        [FromForm] EditVacancyRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var query = new GetVacancyByIdQuery
        {
            VacancyId = request.VacancyId,
        };
        var result = await _mediator.Send(query, cancellationToken);
        var command = new EditVacancyCommand
        {
            Vacancy = result,
            Description = request.Description,
        };
        await _mediator.Send(command, cancellationToken);
        return Ok(command.Vacancy.Id);
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetVacanciesByFilter(
        [FromForm] GetVacancyByFilterRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var query = new GetVacanciesByFilterQuery
        {
            CompnayId = request.CompnayId,
            Title = request.Title,
        };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpGet("{vacancyId:guid}")]
    public async Task<IActionResult> GetVacancy(
        [FromRoute] Guid vacancyId, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var query = new GetVacancyByIdQuery
        {
            VacancyId = vacancyId
        };
        var result = await _mediator.Send(query, cancellationToken);
        var vacancy = _mapper.Map<VacancyDto>(result);
        return Ok(vacancy);
    }
    
    [AllowAnonymous]
    [HttpPost("reply")]
    public async Task<IActionResult> ReplyVacancy(
        [FromForm] ReplyVacancyRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var documentDto = new CandidateDocumentDto
        {
            Name = request.Name,
            Portfolio = request.Portfolio,
            WorkExperience = request.WorkExperience,
        };
        var command = new ReplyVacancyCommand
        {
            VacancyId = request.VacancyId,
            ReferalId = request.ReferalId,
            Document = _mapper.Map<CandidateDocument>(documentDto),
        };
        await _mediator.Send(command, cancellationToken);
        return Ok(command.VacancyId);
    }
}