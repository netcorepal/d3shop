using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Admin.Shared.Responses.MenuResponses;
using NetCorePal.D3Shop.Admin.Shared.Utils;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin
{
    /// <summary>
    /// 菜单查询服务
    /// </summary>
    public class MenuQuery(ApplicationDbContext applicationDbContext) : IQuery
    {
        private DbSet<Menu> MenuSet { get; } = applicationDbContext.Menus;

        /// <summary>
        /// 根据ID获取菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>菜单实体</returns>
        public async Task<MenuTreeNodeResponse?> GetMenuByIdAsync(MenuId id, CancellationToken cancellationToken)
        {
            return await MenuSet.OrderBy(menu => menu.Order)
               .Select(menu => new MenuTreeNodeResponse
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
               })
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        /// <summary>
        /// 获取所有菜单（树形结构）
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>菜单列表</returns>
        public async Task<List<MenuTreeNodeResponse>> GetAllMenusAsync(CancellationToken cancellationToken)
        {
            var menuTreeNode = await MenuSet
               .OrderBy(menu => menu.Order)
               .Select(menu => new MenuTreeNodeResponse
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
               })
               .ToListAsync(cancellationToken);
            return menuTreeNode.ToTree(new MenuId(0));
        }

        /// <summary>
        /// 获取所有菜单（扁平结构）
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>菜单列表</returns>
        public async Task<List<MenusFlatResponse>> GetAllMenusFlatAsync(CancellationToken cancellationToken)
        {
            return await MenuSet
               .OrderBy(menu => menu.Order)
               .Select(menu => new MenusFlatResponse
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
               })
               .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// 根据父ID获取菜单
        /// </summary>
        /// <param name="parentId">父菜单ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>菜单列表</returns>
        public async Task<List<Menu>> GetMenusByParentIdAsync(MenuId parentId, CancellationToken cancellationToken)
        {
            return await MenuSet
                .Where(m => m.ParentId == parentId)
                .OrderBy(m => m.Order)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await MenuSet.AsNoTracking()
                .AnyAsync(m => m.Name == name, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, MenuId excludeId, CancellationToken cancellationToken = default)
        {
            return await MenuSet.AsNoTracking()
                .AnyAsync(m => m.Name == name && m.Id != excludeId, cancellationToken);
        }

        public async Task<bool> ExistsByPathAsync(string path, CancellationToken cancellationToken = default)
        {
            return await MenuSet.AsNoTracking()
                .AnyAsync(m => m.Path == path, cancellationToken);
        }

        public async Task<bool> ExistsByPathAsync(string path, MenuId excludeId, CancellationToken cancellationToken = default)
        {
            return await MenuSet.AsNoTracking()
                .AnyAsync(m => m.Path == path && m.Id != excludeId, cancellationToken);
        }
    }
}
