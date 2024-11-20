using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Admin.Shared.Const;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AdminUserAccountController(
    AdminUserQuery adminUserQuery,
    IOptions<AppConfiguration> appConfiguration) : ControllerBase
{
    private AppConfiguration AppConfiguration => appConfiguration.Value;

    [HttpPost("login")]
    public async Task<ResponseData> LoginAsync([FromBody] AdminUserLoginRequest request)
    {
        var user = await adminUserQuery.GetAdminUserByNameAsync(request.Name, HttpContext.RequestAborted);
        if (user is null)
            throw new KnownException("Invalid Credentials.");

        if (!PasswordHasher.VerifyHashedPassword(request.Password, user.Password))
            throw new KnownException("Invalid Credentials.");

        var claims = GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(AppConfiguration.TokenExpiryInMinutes),
            IsPersistent = request.IsPersistent
        };
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        return new ResponseData();
    }

    private static IEnumerable<Claim> GetClaimsAsync(AdminUser user)
    {
        var roles = user.Roles;
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)).ToList();
        //系统默认用户添加超级管理员角色
        if (user.Name == AppDefaultCredentials.Name)
            roleClaims.Add(new Claim(ClaimTypes.Role, AppClaim.SuperAdminRole));

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.MobilePhone, user.Phone)
            }
            .Union(roleClaims);

        return claims;
    }


    /*private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateJwtAsync(AdminUser user)
    {
        var token = GenerateEncryptedToken(GetSigningCredentials(), GetClaimsAsync(user));
        return token;
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(AppConfiguration.TokenExpiryInMinutes),
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(AppConfiguration.Secret);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }*/
}