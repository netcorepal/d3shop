using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetCorePal.D3Shop.Admin.Shared.Const;

namespace NetCorePal.D3Shop.Web.Tests;

public static class Helpers
{
    public static string GenerateEncryptedToken(IConfiguration configuration)
    {
        var secret = Encoding.UTF8.GetBytes(configuration.GetSection("AppConfiguration").GetValue<string>("Secret") ?? "");
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: [new Claim(ClaimTypes.Role, AppClaim.SuperAdminRole)],
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }
}