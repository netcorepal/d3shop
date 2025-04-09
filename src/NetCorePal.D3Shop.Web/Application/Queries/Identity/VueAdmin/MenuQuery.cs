using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Infrastructure;
using NetCorePal.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Web.Application.Queries
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
        public async Task<Menu?> GetMenuByIdAsync(MenuId id, CancellationToken cancellationToken)
        {
            return await MenuSet
                .Include(m => m.Children)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>菜单列表</returns>
        public async Task<List<Menu>> GetAllMenusAsync(CancellationToken cancellationToken)
        {
            return await MenuSet
            .IgnoreAutoIncludes()
            .Include(m => m.Children)
            .OrderBy(m => m.Order)
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
                .Include(m => m.Children)
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