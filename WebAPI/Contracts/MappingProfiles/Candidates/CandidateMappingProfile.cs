using AutoMapper;
using Domain.Candidates;
using WebApi.Contracts.Dto.Candidates;

namespace WebApi.Contracts.MappingProfiles.Candidates;

public class CandidateMappingProfile : Profile
{
    public CandidateMappingProfile()
    {
        CreateMap<Candidate, CandidateDto>().ReverseMap();
    }
}