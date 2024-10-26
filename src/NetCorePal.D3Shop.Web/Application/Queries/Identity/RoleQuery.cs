using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class RoleQuery(ApplicationDbContext dbContext)
{
    public async Task<List<Role>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Roles.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Role>> GetRolesByCondition(string? name, string? description)
    {
        var queryable = dbContext.Roles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            queryable = queryable.Where(x => x.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(description))
            queryable = queryable.Where(x => x.Description.Contains(description));

        return await queryable.ToListAsync();
    }

    public async Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<Role?> GetRoleByIdAsync(RoleId id, CancellationToken cancellationToken)
    {
        return await dbContext.Roles.FindAsync([id], cancellationToken);
    }
}