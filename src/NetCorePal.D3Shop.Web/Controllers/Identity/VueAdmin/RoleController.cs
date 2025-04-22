using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin;
using NetCorePal.D3Shop.Web.Application.Queries;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using NetCorePal.D3Shop.Admin.Shared.Responses.MenuResponses;

namespace PlaygroundApi.Controllers
{
    [ApiController]
    [Route("api/system/[controller]")]
    [AdminPermission(PermissionCodes.RoleManagement)]
    public class RoleController(IMediator mediator, RoleQuery roleQuery, MenuQuery menuQuery) : ControllerBase
    {

        private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? CancellationToken.None;

        [HttpPost]
        public async Task<ResponseData<RoleId>> CreateRole([FromBody] VueCreateRoleRequest request)
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

            var roleId = await mediator.Send(new VueCreateRoleCommand(request.Name, request.Remark, request.Status, permissions), CancellationToken);
            return roleId.AsResponseData();
        }

        [HttpPut("{id}")]
        public async Task<ResponseData> UpdateRole([FromRoute] RoleId id, [FromBody] VueUpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var menus = await menuQuery.GetAllMenusFlatAsync(cancellationToken);
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

            await mediator.Send(new VueUpdateRoleCommand(
                id,
                request.Name,
                request.Remark,
                request.Status,
                permissions),
                cancellationToken);

            return new ResponseData();
        }

        [HttpGet("list")]
        [AdminPermission(PermissionCodes.RoleView)]
        public async Task<ResponseData<PagedData<VueRoleResponse>>> GetAllRoles([FromQuery] VueRoleQueryRequest request)
        {
            var roles = await roleQuery.GetVueAllRolesAsync(request, CancellationToken);
            return roles.AsResponseData();
        }

    }
}
