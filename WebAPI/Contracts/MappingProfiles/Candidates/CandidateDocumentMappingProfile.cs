using AutoMapper;
using Domain.Candidates;
using WebApi.Contracts.Dto.Candidates;

namespace WebApi.Contracts.MappingProfiles.Candidates;

public class CandidateDocumentMappingProfile : Profile
{
    public CandidateDocumentMappingProfile()
    {
        CreateMap<CandidateDocument, CandidateDocumentDto>();
    }
}