using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class UserQuery(ApplicationDbContext applicationDbContext)
{
    public async Task<AdminUser> LoginAsync(string name, string password)
    {
        var user = await applicationDbContext.AdminUsers.FirstOrDefaultAsync(u =>
            u.Name == name);

        if (user is null)
        {
            throw new KnownException("Invalid Credentials.");
        }

        var isPasswordValid = user.VerifyPassword(password);
        if (!isPasswordValid)
        {
            throw new KnownException("Invalid Credentials.");
        }

        return user;
    }

    public async Task<AdminUser> GetUserByName(string name)
    {
        var user = await applicationDbContext.AdminUsers.FirstOrDefaultAsync(u => u.Name == name) ??
                   throw new KnownException($"Name为{name}的用户不存在");
        return user;
    }
}