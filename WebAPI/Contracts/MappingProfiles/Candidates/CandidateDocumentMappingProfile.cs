using AutoMapper;
using Domain.Candidates;
using WebApi.Contracts.Dto.Candidates;
using WebApi.Contracts.Requests.Vacancies;

namespace WebApi.Contracts.MappingProfiles.Candidates;

public class CandidateDocumentMappingProfile : Profile
{
    public CandidateDocumentMappingProfile()
    {
        CreateMap<ReplyVacancyRequest, CandidateDocument>()
            .ConstructUsing((src) => CandidateDocument.Create(
                src.Name, src.WorkExperience, src.Portfolio))
            .ReverseMap();
    }
}