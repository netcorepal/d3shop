using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class UserQuery(ApplicationDbContext applicationDbContext)
{
    public async Task<AdminUser?> GetUserByName(string name)
    {
        var user = await applicationDbContext.AdminUsers.FirstOrDefaultAsync(u => u.Name == name);

        return user;
    }
}