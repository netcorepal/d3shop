using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;

public class ClientUserQuery(ApplicationDbContext applicationDbContext) : IQuery
{
    private DbSet<ClientUser> ClientUserSet { get; } = applicationDbContext.ClientUsers;

    public async Task<ClientUserAuthInfo> RetrieveClientWithAuthInfoByPhoneAsync(string phoneNumber)
    {
        var authInfo = await ClientUserSet
                           .Where(user => user.Phone == phoneNumber)
                           .Select(user => new ClientUserAuthInfo(user.Id, user.PasswordSalt))
                           .SingleOrDefaultAsync()
                       ?? throw new KnownException("用户不存在");

        return authInfo;
    }
}