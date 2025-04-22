using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Services;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Blazor;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using System.Threading;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin;

[Route("api/[controller]/[action]")]
[ApiController]
[KnownExceptionHandler]
[AdminPermission(PermissionCodes.RoleManagement)]
public class RoleController(IMediator mediator, RoleQuery roleQuery, MenuQuery menuQuery) : ControllerBase, IRolesService
{
    private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? CancellationToken.None;

    [HttpPost]
    [AdminPermission(PermissionCodes.RoleCreate)]
    public async Task<ResponseData<RoleId>> CreateRole([FromBody] CreateRoleRequest request)
    {
        var menus = await menuQuery.GetAllMenusFlatAsync(CancellationToken);
        var permissions = request.Permissions
               .Select(item =>
               {
                   var menu = menus.FirstOrDefault(m => m.Id == item);
                   if (menu == null || string.IsNullOrEmpty(menu.AuthCode))
                   {
                       throw new KnownException("无效的菜单", -1);
                   }
                   return (menu.Id, menu.AuthCode);
               })
               .ToList();

        var roleId = await mediator.Send(new CreateRoleCommand(request.Name, request.Description, request.Status, permissions), CancellationToken);

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
        var menus = await menuQuery.GetAllMenusFlatAsync(CancellationToken);
        var permissions = request.Permissions
               .Select(item =>
               {
                   var menu = menus.FirstOrDefault(m => m.Id == item);
                   if (menu == null || string.IsNullOrEmpty(menu.AuthCode))
                   {
                       throw new KnownException("无效的菜单", -1);
                   }
                   return (menu.Id, menu.AuthCode);
               })
               .ToList();

        await mediator.Send(
            new UpdateRoleInfoCommand(id, request.Name, request.Description,request.Status,permissions),
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
                return permission.Code;
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