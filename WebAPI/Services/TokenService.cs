using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Companies.Models.Services;
using Application.Companies.Models.Submodels;
using Domain.Companies;
using Microsoft.IdentityModel.Tokens;
using WebApi.Options;

namespace WebApi.Services;

public class TokenService : ITokenService
{
    private const int ExpiresDays = 30;
    
    public Token GenerateToken(User user)
    {
        var expiresData = DateTime.Now.AddDays(ExpiresDays);
        
        var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.Name)};
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer, 
            audience:AuthOptions.Audience,
            claims: claims,
            expires: expiresData,
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        var data = new JwtSecurityTokenHandler().WriteToken(jwt);

        var token = Token.Create(data, expiresData);
        return token;
    }
}