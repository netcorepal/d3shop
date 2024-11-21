using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Mappers;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
public class AdminUserController(
    IMediator mediator,
    IMapperProvider mapperProvider,
    AdminUserQuery adminUserQuery,
    RoleQuery roleQuery)
    : ControllerBase
{
    private CancellationToken CancellationToken => HttpContext.RequestAborted;

    private IMapper<AdminUser, AdminUserResponse> AdminUserOutputMapper =>
        mapperProvider.GetMapper<AdminUser, AdminUserResponse>();

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
    [AdminPermission(PermissionDefinitions.AdminUserView)]
    public async Task<ResponseData<IEnumerable<AdminUserResponse>>> GetAllAdminUsers()
    {
        var adminUsers = await adminUserQuery.GetAllAdminUsersAsync(CancellationToken);
        var responses = adminUsers.Select(AdminUserOutputMapper.To);
        return responses.AsResponseData();
    }

    [HttpGet]
    [AdminPermission(PermissionDefinitions.AdminUserView)]
    public async Task<ResponseData<IEnumerable<AdminUserResponse>>> GetAdminUsersByCondition(
        [FromQuery] AdminUserQueryRequest request)
    {
        var adminUsers = await adminUserQuery.GetAdminUsersByCondition(request.Name, request.Phone);
        var responses = adminUsers.Select(AdminUserOutputMapper.To);
        return responses.AsResponseData();
    }

    [HttpGet("{id}")]
    [AdminPermission(PermissionDefinitions.AdminUserView)]
    public async Task<ResponseData<AdminUserResponse>> GetAdminUserById([FromRoute] AdminUserId id)
    {
        var adminUser = await adminUserQuery.GetAdminUserByIdAsync(id, CancellationToken) ??
                        throw new KnownException($"该用户不存在，AdminUserId={id}");

        return AdminUserOutputMapper.To(adminUser).AsResponseData();
    }

    [HttpGet("{id}")]
    [AdminPermission(PermissionDefinitions.AdminUserView)]
    public async Task<ResponseData<List<RoleId>>> GetAssignedRoleIdsForUser([FromRoute] AdminUserId id)
    {
        var roleIds = await adminUserQuery.GetAssignedRoleIdsForUserAsync(id, CancellationToken);
        return roleIds.AsResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionDefinitions.AdminUserUpdatePassword)]
    public async Task<ResponseData> ChangeAdminUserPassword([FromRoute] AdminUserId id,
        [FromBody] UpdateAdminUserPasswordRequest request)
    {
        var adminUser = await adminUserQuery.GetAdminUserByIdAsync(id, CancellationToken);
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
}