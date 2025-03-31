using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Options;

public abstract class AuthOptions
{
    public const string Issuer = "HRSystem"; // издатель токена
    public const string Audience = "HRUser"; // потребитель токена
    private const string Key = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}