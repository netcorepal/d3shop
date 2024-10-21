using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.D3Shop.Web.Controllers.Identity.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.Responses;
using NetCorePal.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using NetCorePal.D3Shop.Web.Helper;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AdminUserTokenController(IMediator mediator, AdminUserQuery adminUserQuery, IOptions<AppConfiguration> appConfiguration) : ControllerBase
{
    private AppConfiguration AppConfiguration => appConfiguration.Value;

    [HttpPost("login")]
    public async Task<ResponseData<AminUserTokenResponse>> LoginAsync([FromBody] AdminUserLoginRequest request)
    {
        var user = await adminUserQuery.GetAdminUserByNameAsync(request.Name, HttpContext.RequestAborted);
        if (user is null)
            throw new KnownException("Invalid Credentials.");

        if (!PasswordHasher.VerifyHashedPassword(request.Password, user.Password))
            throw new KnownException("Invalid Credentials.");

        var refreshToken = GenerateRefreshToken();
        var loginExpiryDate = DateTime.Now.AddDays(7);

        await mediator.Send(new AdminUserLoginSuccessfullyCommand(user.Id, refreshToken, loginExpiryDate));

        var response = new AminUserTokenResponse(GenerateJwtAsync(user), refreshToken, loginExpiryDate);
        return response.AsResponseData();
    }

    [HttpPost("getRefreshToken")]
    public async Task<ResponseData<AminUserTokenResponse>> GetRefreshTokenAsync([FromBody] AdminUserRefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);

        var userName = userPrincipal.FindFirstValue(ClaimTypes.Name) ??
                       throw new KnownException("Invalid Token:There is no username in the token");

        var user = await adminUserQuery.GetAdminUserByNameAsync(userName, HttpContext.RequestAborted);
        if (user is null)
            throw new KnownException("Invalid Token:User does not exist");

        if (string.IsNullOrWhiteSpace(request.RefreshToken) ||
        user.RefreshToken != request.RefreshToken ||
        user.LoginExpiryDate <= DateTime.Now)
            throw new KnownException("Invalid Client Token.");

        var refreshToken = GenerateRefreshToken();
        await mediator.Send(new UpdateAdminUserRefreshTokenCommand(user.Id, refreshToken));

        var response = new AminUserTokenResponse(GenerateJwtAsync(user), refreshToken, user.LoginExpiryDate);
        return response.AsResponseData();
    }

    private static string GenerateRefreshToken()
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

    private static IEnumerable<Claim> GetClaimsAsync(AdminUser user)
    {
        var roles = user.Roles;
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)).ToList();
        //系统默认用户添加超级管理员角色
        if (user.Name == AppDefaultCredentials.Name)
            roleClaims.Add(new Claim(ClaimTypes.Role, AppClaim.SuperAdminRole));

        var permissions = user.Permissions;
        var permissionClaims = permissions.Select(p => new Claim(AppClaim.AdminPermission, p.PermissionCode));

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.MobilePhone, user.Phone)
            }
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
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
    }

}