using AutoMapper;
using Domain.Companies;
using WebApi.Contracts.Dto.Companies;

namespace WebApi.Contracts.MappingProfiles.Companies;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}