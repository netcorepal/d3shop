using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using NetCorePal.Extensions.Jwt;

namespace NetCorePal.D3Shop.Web.Helper;

public class TokenGenerator(IOptions<AppConfiguration> appConfiguration, IJwtProvider jwtProvider)
{
    private AppConfiguration AppConfiguration => appConfiguration.Value;

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ValueTask<string> GenerateJwtAsync(IEnumerable<Claim> claims)
    {
        var token = GenerateEncryptedToken(claims);
        return token;
    }

    private ValueTask<string> GenerateEncryptedToken(IEnumerable<Claim> claims)
    {
        var jwt = jwtProvider.GenerateJwtToken(new JwtData("issuer-x", "audience-y",
            claims,
            DateTime.Now,
            DateTime.Now.AddMinutes(AppConfiguration.TokenExpiryInMinutes)));
        return jwt;
    }
}