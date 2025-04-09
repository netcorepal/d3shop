using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using OpenIddict.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin
{
    [Route("api/auth/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class VueAuthController(
    IMediator mediator,
    AdminUserQuery adminUserQuery,
    TokenGenerator tokenGenerator,
    OpenIddictClientService openIddictClientService,
    IMemoryCache memoryCache) : ControllerBase
    {

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseData<VueUserLoginResponse>> Login([FromBody] VueUserLoginRequest request)
        {
            //重定向地址
            var redirectUri = HttpContext.Request?.Query["redirect"].FirstOrDefault();
            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = "/analytics";
            }
            var authInfo =
                await adminUserQuery.GetUserInfoForLoginAsync(request.UserName, HttpContext.RequestAborted);
            if (authInfo is null)
                throw new KnownException("无效的用户", -1);

            if (!PasswordHasher.VerifyHashedPassword(request.Password, authInfo.Password))
                throw new KnownException("密码错误", -1);

            var refreshToken = TokenGenerator.GenerateRefreshToken();
            ICollection<RoleId> roles = await adminUserQuery.GetAssignedRoleIdsAsync(authInfo.Id, HttpContext.RequestAborted);
            var token = await tokenGenerator.GenerateJwtAsync([
                new Claim(ClaimTypes.NameIdentifier, authInfo.Id.ToString()),
                new Claim(ClaimTypes.Name, authInfo.Name)
            ]);
            return VueUserLoginResponse.Success(token, refreshToken, authInfo.Id, authInfo.Name, authInfo.Name, roles, redirectUri).AsResponseData();
        }

    }
}
