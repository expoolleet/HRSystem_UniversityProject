using AutoMapper;
using Domain.Companies;
using WebApi.Contracts.Dto.Companies;

namespace WebApi.Contracts.MappingProfiles.Companies;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}