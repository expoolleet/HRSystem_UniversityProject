using Application.Companies.Models.Responses;
using AutoMapper;
using WebApi.Contracts.Responses.Companies;

namespace WebApi.Contracts.MappingProfiles.Companies;

public class UserResponseMappingProfile : Profile
{
    public UserResponseMappingProfile()
    {
        CreateMap<UserResponse, AuthUserResponse>().ReverseMap();
    }
}