using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCorePal.Extensions.Jwt;

namespace NetCorePal.D3Shop.Web.Helper;

public class TokenGenerator(
    IOptions<AppConfiguration> appConfiguration,
    IJwtProvider jwtProvider,
    IJwtSettingStore jwtSettingStore)
{
    private AppConfiguration AppConfiguration => appConfiguration.Value;

    public static string GenerateCryptographicallySecureGuid()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[16];
        rng.GetBytes(bytes);
        return new Guid(bytes).ToString("N");
    }

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


    public async ValueTask<ClaimsPrincipal> GetPrincipalFromExpiredToken(
        string token,
        CancellationToken cancellationToken = default)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
            throw new ArgumentException("Invalid JWT format");

        var jwtHeader = handler.ReadJwtToken(token).Header;
        var kid = jwtHeader.Kid;
        if (string.IsNullOrEmpty(kid))
            throw new SecurityTokenValidationException("JWT header missing 'kid'");

        var keySettings = (await jwtSettingStore.GetSecretKeySettings(cancellationToken))
            .Where(s => s.Kid == kid)
            .ToArray();

        if (keySettings.Length == 0)
            throw new SecurityTokenValidationException($"No key found for kid: {kid}");
        var setting = keySettings[0];

        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(setting.PrivateKey), out _);
        var key = new RsaSecurityKey(rsa);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };

        return handler.ValidateToken(token, validationParameters, out _);
    }
}