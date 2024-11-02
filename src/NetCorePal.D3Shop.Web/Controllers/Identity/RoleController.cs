using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Attribute;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.D3Shop.Web.Controllers.Identity.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.Responses;
using NetCorePal.Extensions.Mappers;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
public class RoleController(IMediator mediator, IMapperProvider mapperProvider, RoleQuery roleQuery) : ControllerBase
{
    private CancellationToken CancellationToken => HttpContext.RequestAborted;
    private IMapper<Role, RoleResponse> RoleOutputMapper => mapperProvider.GetMapper<Role, RoleResponse>();

    [HttpPost]
    [MustHaveAdminPermission(PermissionDefinitions.RoleCreate)]
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
    [MustHaveAdminPermission(PermissionDefinitions.RoleView)]
    public async Task<ResponseData<IEnumerable<RoleResponse>>> GetAllRoles()
    {
        var roles = await roleQuery.GetAllRolesAsync(CancellationToken);
        var response = roles.Select(RoleOutputMapper.To);
        return response.AsResponseData();
    }

    [HttpGet]
    [MustHaveAdminPermission(PermissionDefinitions.RoleView)]
    public async Task<ResponseData<IEnumerable<RoleResponse>>> GetRolesByCondition([FromQuery] RoleQueryRequest request)
    {
        var roles = await roleQuery.GetRolesByCondition(request.Name, request.Description);
        var response = roles.Select(RoleOutputMapper.To);
        return response.AsResponseData();
    }

    [HttpGet("{id}")]
    [MustHaveAdminPermission(PermissionDefinitions.RoleView)]
    public async Task<ResponseData<RoleResponse>> GetRoleById([FromRoute] RoleId id)
    {
        var role = await roleQuery.GetRoleByIdAsync(id, CancellationToken);
        if (role == null) throw new KnownException($"未找到角色，RoleId = {id}");

        return RoleOutputMapper.To(role).AsResponseData();
    }

    [HttpGet("{id}")]
    [MustHaveAdminPermission(PermissionDefinitions.RoleView)]
    public async Task<ResponseData<IEnumerable<RolePermissionResponse>>> GetRolePermissions([FromRoute] RoleId id)
    {
        var role = await roleQuery.GetRoleByIdAsync(id, CancellationToken);
        if (role == null) throw new KnownException($"未找到角色，RoleId = {id}");

        var allPermissions = Permissions.AllPermissions;
        var rolePermissionCodes = role.Permissions.Select(x => x.PermissionCode);

        var response = allPermissions.Select(p =>
            rolePermissionCodes.Contains(p.Code)
                ? new RolePermissionResponse(p.Code, p.Remark, p.GroupName, true)
                : new RolePermissionResponse(p.Code, p.Remark, p.GroupName, false));
        return response.AsResponseData();
    }

    [HttpPut("{id}")]
    [MustHaveAdminPermission(PermissionDefinitions.RoleEdit)]
    public async Task<ResponseData> UpdateRoleInfo([FromRoute] RoleId id, [FromBody] UpdateRoleInfoRequest request)
    {
        await mediator.Send(
            new UpdateRoleInfoCommand(id, request.Name, request.Description),
            CancellationToken);

        return new ResponseData();
    }

    [HttpPut("{id}")]
    [MustHaveAdminPermission(PermissionDefinitions.RoleUpdatePermissions)]
    public async Task<ResponseData> UpdateRolePermissions([FromRoute] RoleId id, [FromBody] List<string> permissionCodes)
    {
        var allPermissions = Permissions.AllPermissions;
        var permissionsToBeAssigned = allPermissions.Where(x => permissionCodes.Contains(x.Code))
            .Select(p => new RolePermission(p.Code, p.Remark));

        await mediator.Send(
            new UpdateRolePermissionsCommand(id, permissionsToBeAssigned),
            CancellationToken);
        return new ResponseData();
    }

    [HttpDelete]
    [MustHaveAdminPermission(PermissionDefinitions.RoleDelete)]
    public async Task<ResponseData> DeleteRole([FromRoute] RoleId id)
    {
        await mediator.Send(new DeleteRoleCommand(id), CancellationToken);
        return new ResponseData();
    }
}