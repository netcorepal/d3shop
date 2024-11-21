using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class RoleQuery(ApplicationDbContext dbContext) : IQuery
{
    private DbSet<Role> RoleSet { get; } = dbContext.Roles;
    
    public async Task<List<RoleResponse>> GetAllRolesAsync(RoleQueryRequest query, CancellationToken cancellationToken)
    {
        return await RoleSet
            .WhereIf(!query.Name.IsNullOrWhiteSpace(), r => r.Name.Contains(query.Name!))
            .WhereIf(!query.Description.IsNullOrWhiteSpace(), r => r.Description.Contains(query.Description!))
            .Select(r => new RoleResponse(r.Id, r.Name, r.Description))
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<AssignAdminUserRoleDto>> GetAdminRolesForAssignmentAsync(IEnumerable<RoleId> ids,
        CancellationToken cancellationToken)
    {
        return await RoleSet
            .Where(r => ids.Contains(r.Id))
            .Select(r => new AssignAdminUserRoleDto(
                r.Id,
                r.Name,
                r.Permissions.Select(rp =>
                    new AdminUserPermission(rp.PermissionCode, rp.PermissionRemark)))
            )
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<string>> GetRolePermissionsAsync(RoleId id, CancellationToken cancellationToken)
    {
        return await RoleSet.Where(r => r.Id == id)
            .SelectMany(r => r.Permissions.Select(rp => rp.PermissionCode))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> RoleExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await RoleSet.AnyAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<bool> RoleExistsByNameAsync(string name, RoleId id, CancellationToken cancellationToken)
    {
        return await RoleSet.AnyAsync(r => r.Name == name && r.Id != id, cancellationToken);
    }
}