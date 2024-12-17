using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.D3Shop.Web.Controllers.Identity.Dto;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class AdminUserQuery(ApplicationDbContext applicationDbContext, IMemoryCache memoryCache) : IQuery
{
    private DbSet<AdminUser> AdminUserSet { get; } = applicationDbContext.AdminUsers;

    public async Task<AdminUserCredentials?> GetUserCredentialsIfExists(AdminUserId id,
        CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .Where(au => au.Id == id)
            .Select(au => new AdminUserCredentials(au.Id, au.Password))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<RoleId>> GetAssignedRoleIdsAsync(AdminUserId id, CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .Where(u => u.Id == id)
            .SelectMany(u => u.Roles.Select(ur => ur.RoleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AdminUserAssignedPermissionResponse>> GetAssignedPermissionsAsync(AdminUserId id,
        CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .Where(au => au.Id == id)
            .SelectMany(au => au.Permissions)
            .Select(aup => new AdminUserAssignedPermissionResponse(aup.PermissionCode, aup.SourceRoleIds.Count > 0))
            .ToListAsync(cancellationToken);
    }

    public async Task<AuthenticationUserInfo?> GetUserInfoForLoginAsync(string name,
        CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .Where(au => au.Name == name)
            .Select(au => new AuthenticationUserInfo(au.Id, au.Name, au.Password, au.Phone))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<PagedData<AdminUserResponse>> GetAllAdminUsersAsync(AdminUserQueryRequest queryRequest,
        CancellationToken cancellationToken)
    {
        var adminUsers = await AdminUserSet.AsNoTracking()
            .WhereIf(!queryRequest.Name.IsNullOrWhiteSpace(), au => au.Name.Contains(queryRequest.Name!))
            .WhereIf(!queryRequest.Phone.IsNullOrWhiteSpace(), au => au.Phone.Contains(queryRequest.Phone!))
            .OrderBy(au => au.Id)
            .Select(au => new AdminUserResponse(au.Id, au.Name, au.Phone, au.Roles.Select(r => r.RoleName)))
            .ToPagedDataAsync(queryRequest, cancellationToken);
        return adminUsers;
    }

    public async Task<List<AdminUserId>> GetAdminUserIdsByRoleIdAsync(RoleId roleId,
        CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .Where(x => x.Roles.Any(r => r.RoleId == roleId))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DoesAdminUserExist(string userName, CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .AnyAsync(au => au.Name == userName, cancellationToken);
    }

    public async Task<List<AdminUserId>> GetUserIdsByDeptIdAsync(DeptId deptId, CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .Where(x => x.UserDept.DeptId == deptId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);
    }
    public async Task<bool> DoesAdminUserExist(RoleId roleId, CancellationToken cancellationToken)
    {
        return await AdminUserSet.AsNoTracking()
            .AnyAsync(x => x.Roles.Any(r => r.RoleId == roleId), cancellationToken: cancellationToken);
    }

    public async Task<List<string>?> GetAdminUserPermissionCodes(AdminUserId id)
    {
        var cacheKey = $"{CacheKeys.AdminUserPermissions}:{id}";
        var adminUserPermissions = await memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await AdminUserSet.AsNoTracking()
                .Where(au => au.Id == id)
                .SelectMany(au => au.Permissions.Select(p => p.PermissionCode))
                .ToListAsync();
        });

        return adminUserPermissions;
    }
}