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

    public async Task<List<ClientUserDeliveryAddressInfo>> GetDeliveryAddressesAsync(ClientUserId userId)
    {
        return await ClientUserSet
            .Where(user => user.Id == userId)
            .SelectMany(user => user.DeliveryAddresses)
            .Select(address => new ClientUserDeliveryAddressInfo(
                address.Id,
                address.Address,
                address.RecipientName,
                address.Phone,
                address.IsDefault
            ))
            .ToListAsync();
    }

    public async Task<string> GetUserPasswordSaltByIdAsync(ClientUserId userId)
    {
        var salt = await ClientUserSet
                       .Where(user => user.Id == userId)
                       .Select(user => user.PasswordSalt)
                       .SingleOrDefaultAsync()
                   ?? throw new KnownException("用户不存在");

        return salt;
    }

    public async Task<List<ClientUserThirdPartyLoginInfo>> GetThirdPartyLoginsAsync(ClientUserId userId)
    {
        return await ClientUserSet
            .Where(user => user.Id == userId)
            .SelectMany(user => user.ThirdPartyLogins.Select(
                    thirdPartyLogin => new ClientUserThirdPartyLoginInfo(
                        thirdPartyLogin.Id,
                        thirdPartyLogin.Provider)
                )
            )
            .ToListAsync();
    }
}