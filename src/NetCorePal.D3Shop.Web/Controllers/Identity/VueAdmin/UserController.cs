using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using MediatR;
using NetCorePal.Extensions.Dto;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses;

namespace PlaygroundApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AdminPermission(PermissionCodes.AdminUserManagement)]
    public class UserController(IMediator mediator, AdminUserQuery adminUserQuery, RoleQuery roleQuery, ICurrentVueAdminUser currentUser) : ControllerBase
    {
        private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;

        [HttpGet("info")]
        public async Task<ResponseData<VueAdminUserResponse?>> GetUserInfo()
        {
            var userId = currentUser.UserId;
            var adminUsers = await adminUserQuery.GetAdminUserByIdAsync(userId, CancellationToken);
            return adminUsers.AsResponseData();
        }


        [HttpGet("/api/auth/codes")]
        [AdminPermission(PermissionCodes.AdminUserCreate)]
        public async Task<ActionResult<object>> GetAccessCodes()
        {
            try
            {
                //从请求中获取用户ID
                var userId = currentUser.UserId;

                var codes = await adminUserQuery.GetAdminUserPermissionCodes(userId);

                return Ok(new { code = 0, data = codes, error = "", message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = -1, message = "服务器内部错误", error = ex.Message });
            }
        }

        [HttpPost("/api/auth/logout")]
        public ActionResult<object> Logout()
        {
            try
            {
                // 清除Refresh Token Cookie
                Response.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                return Ok(new { code = 0, data = new { }, error = "", message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = -1, message = "服务器内部错误", error = ex.Message });
            }
        }
    }
}
