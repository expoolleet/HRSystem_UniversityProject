using Application.Companies.Models.Submodels;
using AutoMapper;
using WebApi.Contracts.Dto.Companies;

namespace WebApi.Contracts.MappingProfiles.Companies;

public class TokenMappingProfile : Profile
{
    public TokenMappingProfile()
    {
        CreateMap<Token, TokenDto>().ReverseMap();
    }
}