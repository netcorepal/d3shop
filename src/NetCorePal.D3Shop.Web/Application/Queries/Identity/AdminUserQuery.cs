using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class AdminUserQuery(ApplicationDbContext applicationDbContext, IMemoryCache memoryCache) : IQuery
{
    public async Task<AdminUser?> GetAdminUserByIdAsync(AdminUserId id, CancellationToken cancellationToken)
    {
        return await applicationDbContext.AdminUsers.FindAsync([id], cancellationToken);
    }
    
    public async Task<List<RoleId>> GetAssignedRoleIdsForUserAsync(AdminUserId id, CancellationToken cancellationToken)
    {
        return await applicationDbContext.AdminUsers
            .Where(u => u.Id == id)
            .SelectMany(u => u.Roles.Select(ur => ur.RoleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AdminUser>> GetAdminUsersByCondition(string? name, string? phone)
    {
        var queryable = applicationDbContext.AdminUsers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            queryable = queryable.Where(x => x.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(phone))
            queryable = queryable.Where(x => x.Phone.Contains(phone));

        return await queryable.ToListAsync();
    }

    public async Task<AdminUser?> GetAdminUserByNameAsync(string name, CancellationToken cancellationToken)
    {
        var adminUser =
            await applicationDbContext.AdminUsers.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

        return adminUser;
    }

    public async Task<List<AdminUser>> GetAllAdminUsersAsync(CancellationToken cancellationToken)
    {
        var adminUsers = await applicationDbContext.AdminUsers.ToListAsync(cancellationToken);
        return adminUsers;
    }

    public async Task<List<AdminUser>> GetAdminUserByRoleIdAsync(RoleId roleId, CancellationToken cancellationToken)
    {
        var adminUsers = await applicationDbContext.AdminUsers
            .Where(x => x.Roles.Any(r => r.RoleId == roleId))
            .ToListAsync(cancellationToken);
        return adminUsers;
    }

    public async Task<List<string>?> GetAdminUserPermissionCodes(AdminUserId id)
    {
        var cacheKey = $"{CacheKeys.AdminUserPermissions}:{id}";
        var adminUserPermissions = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await applicationDbContext.AdminUsers
                .Where(au => au.Id == id)
                .SelectMany(au => au.Permissions.Select(p => p.PermissionCode))
                .ToListAsync();
        });
        
        return adminUserPermissions;
    }
}