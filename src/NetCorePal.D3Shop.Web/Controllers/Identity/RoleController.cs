using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
[KnownExceptionHandler]
[AdminPermission(PermissionCodes.RoleManagement)]
public class RoleController(IMediator mediator, RoleQuery roleQuery) : ControllerBase, IRolesService
{
    private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? CancellationToken.None;

    [HttpPost]
    [AdminPermission(PermissionCodes.RoleCreate)]
    public async Task<ResponseData<RoleId>> CreateRole([FromBody] CreateRoleRequest request)
    {
        var permissionsToBeAssigned = request.PermissionCodes
            .Select(code =>
            {
                var permission = PermissionDefinitionContext.GetPermission(code);
                return new RolePermissionDto(permission.Code, permission.DisplayName);
            });

        var roleId = await mediator.Send(
            new CreateRoleCommand(request.Name, request.Description, permissionsToBeAssigned),
            CancellationToken);

        return roleId.AsResponseData();
    }

    [HttpGet]
    [AdminPermission(PermissionCodes.RoleView)]
    public async Task<ResponseData<PagedData<RoleResponse>>> GetAllRoles([FromQuery] RoleQueryRequest request)
    {
        var roles = await roleQuery.GetAllRolesAsync(request, CancellationToken);
        return roles.AsResponseData();
    }

    [HttpGet("{id}")]
    [AdminPermission(PermissionCodes.RoleView)]
    public async Task<ResponseData<List<string>>> GetAssignedPermissionCodes([FromRoute] RoleId id)
    {
        var rolePermissions = await roleQuery.GetAssignedPermissionCodes(id, CancellationToken);
        return rolePermissions.AsResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionCodes.RoleEdit)]
    public async Task<ResponseData> UpdateRoleInfo([FromRoute] RoleId id, [FromBody] UpdateRoleInfoRequest request)
    {
        await mediator.Send(
            new UpdateRoleInfoCommand(id, request.Name, request.Description),
            CancellationToken);

        return new ResponseData();
    }

    [HttpPut("{id}")]
    [AdminPermission(PermissionCodes.RoleUpdatePermissions)]
    public async Task<ResponseData> UpdateRolePermissions([FromRoute] RoleId id,
        [FromBody] IEnumerable<string> permissionCodes)
    {
        var permissionsToBeAssigned = permissionCodes
            .Select(code =>
            {
                var permission = PermissionDefinitionContext.GetPermission(code);
                return new RolePermissionDto(permission.Code, permission.DisplayName);
            });

        await mediator.Send(
            new UpdateRolePermissionsCommand(id, permissionsToBeAssigned),
            CancellationToken);
        return new ResponseData();
    }

    [HttpDelete("{id}")]
    [AdminPermission(PermissionCodes.RoleDelete)]
    public async Task<ResponseData> DeleteRole([FromRoute] RoleId id)
    {
        await mediator.Send(new DeleteRoleCommand(id), CancellationToken);
        return new ResponseData();
    }
}