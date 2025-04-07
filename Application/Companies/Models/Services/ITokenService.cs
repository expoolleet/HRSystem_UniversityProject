using Application.Companies.Models.Submodels;
using Domain.Companies;

namespace Application.Companies.Models.Services;

public interface ITokenService
{
    Token GenerateToken(User user, Role role);
}