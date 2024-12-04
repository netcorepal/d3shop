using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
[AdminPermission(PermissionDefinitions.AdminUserView)]
public class AdminUserController(
    IMediator mediator,
    AdminUserQuery adminUserQuery,
    RoleQuery roleQuery)
    : ControllerBase
{
    private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;

    [HttpPost]
    [AdminPermission(PermissionDefinitions.AdminUserCreate)]
    public async Task<ResponseData<AdminUserId>> CreateAdminUser([FromBody] CreateAdminUserRequest request)
    {
        var rolesToBeAssigned = await roleQuery.GetAdminRolesForAssignmentAsync(request.RoleIds, CancellationToken);

        var password = PasswordHasher.HashPassword(request.PassWord);
        var adminUserId = await mediator.Send(
            new CreateAdminUserCommand(request.Name, request.Phone, password, rolesToBeAssigned),
            CancellationToken);

        return adminUserId.AsResponseData();
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
    public async Task<ResponseData<List<AdminUserPermissionResponse>>> GetAdminUserPermissions(
        [FromRoute] AdminUserId id)
    {
        var allPermissions = Permissions.AllPermissions;
        var assignedPermissions = await adminUserQuery.GetAssignedPermissionsAsync(id, CancellationToken);
        var response = allPermissions.Select(p =>
            {
                var assigned = assignedPermissions.Find(ap => ap.PermissionCode == p.Code);
                var isAssigned = assigned is not null;
                var isFromRole = assigned?.SourceRoleIds.Count > 0;
                return new AdminUserPermissionResponse(p.Code, p.GroupName, p.Remark, isAssigned, isFromRole);
            }
        ).ToList();
        return response.AsResponseData();
    }

    [HttpPut("{id}")]
    public async Task<ResponseData> SetAdminUserSpecificPermissions(AdminUserId id,
        [FromBody] IEnumerable<string> permissionCodes)
    {
        var allPermissions = Permissions.AllPermissions;
        var permissionsToBeAssigned = allPermissions.Where(x => permissionCodes.Contains(x.Code))
            .Select(p => new AdminUserPermissionDto(p.Code, p.Remark));
        await mediator.Send(new SetAdminUserSpecificPermissions(id, permissionsToBeAssigned), CancellationToken);
        return new ResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionDefinitions.AdminUserUpdatePassword)]
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

    [HttpPut("{id}")]
    [AdminPermission(PermissionDefinitions.AdminUserUpdateRoles)]
    public async Task<ResponseData> UpdateAdminUserRoles([FromRoute] AdminUserId id,
        [FromBody] IEnumerable<RoleId> roleIds)
    {
        var rolesToBeAssigned = await roleQuery.GetAdminRolesForAssignmentAsync(roleIds, CancellationToken);
        await mediator.Send(new UpdateAdminUserRolesCommand(id, rolesToBeAssigned), CancellationToken);
        return new ResponseData();
    }

    [HttpDelete("{id}")]
    [AdminPermission(PermissionDefinitions.AdminUserDelete)]
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
}