using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
[KnownExceptionHandler]
public class RoleController(IMediator mediator, RoleQuery roleQuery) : ControllerBase, IRolesService
{
    private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? CancellationToken.None;

    [HttpPost]
    [AdminPermission(PermissionDefinitions.RoleCreate)]
    public async Task<ResponseData<RoleId>> CreateRole([FromBody] CreateRoleRequest request)
    {
        var allPermissions = Permissions.AllPermissions;
        var permissionsToBeAssigned = allPermissions.Where(x => request.PermissionCodes.Contains(x.Code))
            .Select(p => new RolePermission(p.Code, p.Remark));

        var roleId = await mediator.Send(
            new CreateRoleCommand(request.Name, request.Description, permissionsToBeAssigned),
            CancellationToken);

        return roleId.AsResponseData();
    }

    [HttpGet]
    [AdminPermission(PermissionDefinitions.RoleView)]
    public async Task<ResponseData<PagedData<RoleResponse>>> GetAllRoles([FromQuery] RoleQueryRequest request)
    {
        var roles = await roleQuery.GetAllRolesAsync(request, CancellationToken);
        return roles.AsResponseData();
    }

    [HttpGet("{id}")]
    [AdminPermission(PermissionDefinitions.RoleView)]
    public async Task<ResponseData<List<RolePermissionResponse>>> GetRolePermissions([FromRoute] RoleId id)
    {
        var allPermissions = Permissions.AllPermissions;
        var rolePermissions = await roleQuery.GetRolePermissionsAsync(id, CancellationToken);
        var response = allPermissions.Select(p =>
                rolePermissions.Contains(p.Code)
                    ? new RolePermissionResponse(p.Code, p.GroupName, p.Remark, true)
                    : new RolePermissionResponse(p.Code, p.GroupName, p.Remark, false))
            .ToList();
        return response.AsResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionDefinitions.RoleEdit)]
    public async Task<ResponseData> UpdateRoleInfo([FromRoute] RoleId id, [FromBody] UpdateRoleInfoRequest request)
    {
        await mediator.Send(
            new UpdateRoleInfoCommand(id, request.Name, request.Description),
            CancellationToken);

        return new ResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionDefinitions.RoleUpdatePermissions)]
    public async Task<ResponseData> UpdateRolePermissions([FromRoute] RoleId id,
        [FromBody] List<string> permissionCodes)
    {
        var allPermissions = Permissions.AllPermissions;
        var permissionsToBeAssigned = allPermissions.Where(x => permissionCodes.Contains(x.Code))
            .Select(p => new RolePermission(p.Code, p.Remark));

        await mediator.Send(
            new UpdateRolePermissionsCommand(id, permissionsToBeAssigned),
            CancellationToken);
        return new ResponseData();
    }

    [HttpDelete("{id}")]
    [AdminPermission(PermissionDefinitions.RoleDelete)]
    public async Task<ResponseData> DeleteRole([FromRoute] RoleId id)
    {
        await mediator.Send(new DeleteRoleCommand(id), CancellationToken);
        return new ResponseData();
    }

    [HttpGet]
    public Task<ResponseData<IEnumerable<RolePermissionResponse>>> GetAllPermissionsForCreateRole()
    {
        IEnumerable<Permission> permissions = Permissions.AllPermissions;
        var response = permissions.Select(p => new RolePermissionResponse(p.Code, p.GroupName, p.Remark, false));
        return Task.FromResult(response.AsResponseData());
    }
}