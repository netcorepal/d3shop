using Microsoft.AspNetCore.Mvc;
using MediatR;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Web.Application.Queries;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.VueAdmin;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests;
using NetCorePal.D3Shop.Admin.Shared.Responses.MenuResponses;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin
{
    /// <summary>
    /// 菜单
    /// </summary>
    [ApiController]
    [Route("api/system/[controller]")]
    [AdminPermission(PermissionCodes.MenuManagement)]
    public class MenuController(IMediator _mediator, MenuQuery _menuQuery) : ControllerBase
    {

        private CancellationToken CancellationToken => HttpContext?.RequestAborted ?? default;

        /// <summary>
        /// 检查菜单名称是否已存在
        /// </summary>
        /// <param name="name">菜单名称</param>
        /// <param name="id">当前菜单ID（修改时使用）</param>
        /// <returns>是否存在</returns>
        [HttpGet("name-exists")]
        public async Task<ActionResult<ResponseData<bool>>> CheckNameExists([FromQuery] string? name, [FromQuery] MenuId? id)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new KnownException("菜单名称不能为空", -1);
            }
            bool exists;
            if (id != null)
            {
                exists = await _menuQuery.ExistsByNameAsync(name, id, CancellationToken);
            }
            else
            {
                exists = await _menuQuery.ExistsByNameAsync(name, CancellationToken);
            }
            if (exists)
            {
                throw new KnownException("菜单名称已存在", -1);
            }
            return exists.AsResponseData();


        }

        /// <summary>
        /// 检查菜单路径是否已存在
        /// </summary>
        /// <param name="path">菜单路径</param>
        /// <param name="id">当前菜单ID（修改时使用）</param>
        /// <returns>是否存在</returns>
        [HttpGet("path-exists")]
        public async Task<ActionResult<ResponseData<bool>>> CheckPathExists([FromQuery] string? path, [FromQuery] MenuId? id)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new KnownException("菜单路径不能为空", -1);
            }

            bool exists;
            if (id != null)
            {
                exists = await _menuQuery.ExistsByPathAsync(path, id, CancellationToken);
            }
            else
            {
                exists = await _menuQuery.ExistsByPathAsync(path, CancellationToken);
            }
            if (exists)
            {
                throw new KnownException("菜单路径已存在", -1);
            }
            return exists.AsResponseData();
        }

        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        /// <returns>菜单列表</returns>
        [HttpGet("list")]
        public async Task<ResponseData<List<MenuTreeNodeResponse>>> GetMenuList()
        {
            var menuTreeNode = await _menuQuery.GetAllMenusAsync(CancellationToken);
            return menuTreeNode.AsResponseData();
        }

        /// <summary>
        /// 根据ID获取菜单详情
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>菜单详情</returns>
        [HttpGet("{id}")]
        public async Task<ResponseData<MenuTreeNodeResponse>> GetMenu(MenuId id)
        {
            var menu = await _menuQuery.GetMenuByIdAsync(id, CancellationToken);
            if (menu is null)
            {
                throw new KnownException("菜单不存在", -1);
            }

            return new ResponseData<MenuTreeNodeResponse>(MapToDto(menu), true, "success", 0, null);
        }

        /// <summary>
        /// 创建新菜单
        /// </summary>
        /// <param name="request">创建菜单请求</param>
        /// <returns>创建结果</returns>
        [HttpPost]
        public async Task<ResponseData<MenuId>> CreateMenu([FromBody] CreateMenuRequest request)
        {
            try
            {
                var menuId = await _mediator.Send(new CreateMenuCommand(
                    request.Name,
                    request.Path,
                    request.Type,
                    request.Pid,
                    request.AuthCode,
                    request.Component,
                    request.Redirect,
                    request.Order,
                    request.Icon,
                    request.Status,
                    request.Meta
                ), CancellationToken);

                return menuId.AsResponseData();
            }
            catch (InvalidOperationException ex)
            {
                throw new KnownException(ex.Message, 400);
            }
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="request">更新菜单请求</param>
        /// <returns>更新结果</returns>
        [HttpPut("{id}")]
        public async Task<ResponseData<MenuId>> UpdateMenu(MenuId id, [FromBody] UpdateMenuRequest request)
        {
            try
            {
                await _mediator.Send(new UpdateMenuCommand(
                    id,
                    request.Name,
                    request.Path,
                    request.Type,
                    request.Pid,
                    request.AuthCode,
                    request.Component,
                    request.Redirect,
                    request.Order,
                    request.Icon,
                    request.Status,
                    request.Meta
                ), CancellationToken);

                return id.AsResponseData();
            }
            catch (InvalidOperationException ex)
            {
                throw new KnownException(ex.Message, 400);
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id}")]
        public async Task<ResponseData<MenuId>> DeleteMenu(MenuId id)
        {
            try
            {
                await _mediator.Send(new DeleteMenuCommand(id), CancellationToken);
                return id.AsResponseData();
            }
            catch (InvalidOperationException ex)
            {
                throw new KnownException(ex.Message, 400);
            }
        }


        /// <summary>
        /// 将Menu实体转换为MenuDto
        /// </summary>
        /// <param name="menu">菜单实体</param>
        /// <returns>菜单DTO</returns>
        private static MenuTreeNodeResponse MapToDto(Menu menu)
        {
            var response = new MenuTreeNodeResponse
            {
                Id = menu.Id,
                Pid = menu.ParentId,
                Name = menu.Name,
                Path = menu.Path,
                Component = menu.Component,
                Icon = menu.Icon,
                Status = menu.Status,
                Redirect = menu.Redirect,
                Type = menu.Type.ToString().ToLower(),
                AuthCode = menu.AuthCode,
                Meta = new MenuMeta
                {
                    Title = menu.Name,
                    Icon = menu.Icon,
                    Order = menu.Order,
                    HideInMenu = !menu.IsVisible,
                    HideInTab = !menu.IsEnabled,
                    KeepAlive = true,
                    AffixTab = false,
                    HideInBreadcrumb = false,
                    HideChildrenInMenu = false,
                    OpenInNewWindow = false,
                    NoBasicLayout = false,
                    MaxNumOfOpenTab = 10
                }
            };

            //// 递归转换子菜单，避免循环引用
            //if (menu.Children != null && menu.Children.Any())
            //{
            //    response.Children = menu.Children.ToList();
            //}
            //else
            //{
            //    response.Children = new List<MenuTreeNodeResponse>();
            //}
            return response;
        }

        /// <summary>
        /// 设置菜单可见性
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="request">可见性设置请求</param>
        /// <returns>设置结果</returns>
        [HttpPut("{id}/visibility")]
        public async Task<ResponseData<object>> SetMenuVisibility(long id, [FromBody] SetVisibilityRequest request)
        {
            try
            {
                await _mediator.Send(new SetMenuVisibilityCommand(new MenuId(id), request.IsVisible), CancellationToken);
                return new ResponseData<object>(new object(), true, "Menu visibility updated successfully", 0, null);
            }
            catch (InvalidOperationException ex)
            {
                throw new KnownException(ex.Message, 400);
            }
        }

        /// <summary>
        /// 设置菜单启用状态
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="request">启用状态设置请求</param>
        /// <returns>设置结果</returns>
        [HttpPut("{id}/enabled")]
        public async Task<ResponseData<object>> SetMenuEnabled(long id, [FromBody] SetEnabledRequest request)
        {
            try
            {
                await _mediator.Send(new SetMenuEnabledCommand(new MenuId(id), request.IsEnabled), CancellationToken);
                return new ResponseData<object>(new object(), true, "Menu enabled status updated successfully", 0, null);
            }
            catch (InvalidOperationException ex)
            {
                throw new KnownException(ex.Message, 400);
            }
        }


    }
}
