using Domain.Companies;

namespace Application.Companies.Models.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}