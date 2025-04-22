using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin;

[Route("api/[controller]/[action]")]
[ApiController]
[KnownExceptionHandler]
[AdminPermission(PermissionCodes.AdminUserManagement)]
public class AdminUserController(
    IMediator mediator,
    AdminUserQuery adminUserQuery,
    ICurrentAdminUser currentUser,
    RoleQuery roleQuery)
    : ControllerBase, IAdminUserService
{
    private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;

    [HttpPost]
    [AdminPermission(PermissionCodes.AdminUserCreate)]
    public async Task<ResponseData<AdminUserId>> CreateAdminUser([FromBody] CreateAdminUserRequest request)
    {
        var rolesToBeAssigned = await roleQuery.GetAdminRolesForAssignmentAsync(request.RoleIds, CancellationToken);

        var password = PasswordHasher.HashPassword(request.PassWord);
        var adminUserId = await mediator.Send(
            new CreateAdminUserCommand(request.Name, request.Phone, password, rolesToBeAssigned),
            CancellationToken);

        return adminUserId.AsResponseData();
    }

    /// <summary>
    /// 获取当前登录用户的信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResponseData<AdminUserResponse?>> GetUserInfo()
    {
        var userId = currentUser.UserId;
        var adminUsers = await adminUserQuery.GetAdminUserByIdAsync(userId, CancellationToken);
        return adminUsers.AsResponseData();
    }


    /// <summary>
    ///  获取当前登录用户的权限代码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResponseData<List<string>?>> GetAccessCodes()
    {
        //从请求中获取用户ID
        var userId = currentUser.UserId;
        var codes = await adminUserQuery.GetAdminUserPermissionCodes(userId);
        return codes.AsResponseData();

    }

    [HttpGet]
    public async Task<ResponseData<PagedData<AdminUserResponse>>> GetAllAdminUsers(
        [FromQuery] AdminUserQueryRequest request)
    {
        var adminUsers = await adminUserQuery.GetAllAdminUsersAsync(request, CancellationToken);
        return adminUsers.AsResponseData();
    }

    [HttpGet("{id}")]
    public async Task<ResponseData<List<AdminUserRoleResponse>>> GetAdminUserRoles([FromRoute] AdminUserId id)
    {
        var allRoles = await roleQuery.GetAllAdminUserRolesAsync(CancellationToken);
        var assignedRoleIds = await adminUserQuery.GetAssignedRoleIdsAsync(id, CancellationToken);
        var response = allRoles.Select(r =>
        {
            if (assignedRoleIds.Contains(r.RoleId))
                r.IsAssigned = true;
            return r;
        }).ToList();
        return response.AsResponseData();
    }

    [HttpGet("{id}")]
    public async Task<ResponseData<List<AdminUserAssignedPermissionResponse>>> GetAssignedPermissions(
        [FromRoute] AdminUserId id)
    {
        var assignedPermissions = await adminUserQuery.GetAssignedPermissionsAsync(id, CancellationToken);
        return assignedPermissions.AsResponseData();
    }

    [HttpPut("{id}")]
    public async Task<ResponseData> SetAdminUserSpecificPermissions(AdminUserId id,
        [FromBody] IEnumerable<string> permissionCodes)
    {
        var permissionsToBeAssigned = permissionCodes
            .Select(code =>
            {
                var permission = PermissionDefinitionContext.GetPermission(code);
                return permission.Code;
            });
        await mediator.Send(new SetAdminUserSpecificPermissions(id, permissionsToBeAssigned), CancellationToken);
        return new ResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionCodes.AdminUserUpdateRoles)]
    public async Task<ResponseData> UpdateAdminUserRoles([FromRoute] AdminUserId id,
        [FromBody] IEnumerable<RoleId> roleIds)
    {
        var rolesToBeAssigned = await roleQuery.GetAdminRolesForAssignmentAsync(roleIds, CancellationToken);
        await mediator.Send(new UpdateAdminUserRolesCommand(id, rolesToBeAssigned), CancellationToken);
        return new ResponseData();
    }

    [HttpDelete("{id}")]
    [AdminPermission(PermissionCodes.AdminUserDelete)]
    public async Task<ResponseData> DeleteAdminUser([FromRoute] AdminUserId id)
    {
        await mediator.Send(new DeleteAdminUserCommand(id), CancellationToken);
        return new ResponseData();
    }

    [HttpGet]
    public async Task<ResponseData<List<AdminUserRoleResponse>>> GetAllRolesForCreateUser()
    {
        var roles = await roleQuery.GetAllAdminUserRolesAsync(CancellationToken);
        return roles.AsResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionCodes.AdminUserUpdatePassword)]
    public async Task<ResponseData> ChangeAdminUserPassword([FromRoute] AdminUserId id,
        [FromBody] UpdateAdminUserPasswordRequest request)
    {
        var adminUser = await adminUserQuery.GetUserCredentialsIfExists(id, CancellationToken);
        if (adminUser is null) throw new KnownException($"该用户不存在，AdminUserId = {id}");

        if (!PasswordHasher.VerifyHashedPassword(adminUser.Password, request.OldPassword))
            throw new KnownException("旧密码不正确");

        var password = PasswordHasher.HashPassword(request.NewPassword);
        await mediator.Send(new UpdateAdminUserPasswordCommand(adminUser.Id, password), CancellationToken);
        return new ResponseData();
    }
}