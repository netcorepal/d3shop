using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;

public class ClientUserQuery(ApplicationDbContext applicationDbContext) : IQuery
{
    private DbSet<ClientUser> ClientUserSet { get; } = applicationDbContext.ClientUsers;

    public async Task<ClientUserAuthInfo> RetrieveClientWithAuthInfoByPhoneAsync(string phoneNumber,
        CancellationToken cancellationToken)
    {
        var authInfo = await ClientUserSet.AsNoTracking()
                           .Where(user => user.Phone == phoneNumber)
                           .Select(user => new ClientUserAuthInfo(user.Id, user.PasswordSalt))
                           .SingleOrDefaultAsync(cancellationToken)
                       ?? throw new KnownException("用户不存在");

        return authInfo;
    }

    public async Task<ClientUserInfo> GetClientUserInfoByIdAsync(ClientUserId userId,
        CancellationToken cancellationToken)
    {
        var userInfo = await ClientUserSet.AsNoTracking()
                           .Where(user => user.Id == userId)
                           .Select(user => new ClientUserInfo(
                               user.Id,
                               user.NickName,
                               user.Avatar,
                               user.Phone,
                               user.Email
                           ))
                           .SingleOrDefaultAsync(cancellationToken)
                       ?? throw new KnownException("用户不存在");

        return userInfo;
    }

    public async Task<List<ClientUserDeliveryAddressInfo>> GetDeliveryAddressesAsync(ClientUserId userId,
        CancellationToken cancellationToken)
    {
        return await ClientUserSet.AsNoTracking()
            .Where(user => user.Id == userId)
            .SelectMany(user => user.DeliveryAddresses)
            .Select(address => new ClientUserDeliveryAddressInfo(
                address.Id,
                address.Address,
                address.RecipientName,
                address.Phone,
                address.IsDefault
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<string> GetUserPasswordSaltByIdAsync(ClientUserId userId, CancellationToken cancellationToken)
    {
        var salt = await ClientUserSet.AsNoTracking()
                       .Where(user => user.Id == userId)
                       .Select(user => user.PasswordSalt)
                       .SingleOrDefaultAsync(cancellationToken)
                   ?? throw new KnownException("用户不存在");

        return salt;
    }

    public async Task<List<ClientUserThirdPartyLoginInfo>> GetThirdPartyLoginsAsync(ClientUserId userId,
        CancellationToken cancellationToken)
    {
        return await ClientUserSet.AsNoTracking()
            .Where(user => user.Id == userId)
            .SelectMany(user => user.ThirdPartyLogins.Select(
                    thirdPartyLogin => new ClientUserThirdPartyLoginInfo(
                        thirdPartyLogin.Id,
                        thirdPartyLogin.Provider)
                )
            )
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DoesPhoneExistAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await ClientUserSet.AsNoTracking()
            .AnyAsync(user => user.Phone == phoneNumber, cancellationToken);
    }

    public async Task<ClientUserId?> GetClientUserIdByOpenIdAsync(string openId, CancellationToken cancellationToken)
    {
        return await ClientUserSet.AsNoTracking()
            .Where(u => u.ThirdPartyLogins.Any(t => t.OpenId == openId))
            .Select(u => u.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<ClientUserId> GetClientUserIdByRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken)
    {
        return await ClientUserSet.AsNoTracking()
                   .SelectMany(u => u.RefreshTokens)
                   .Where(t => t.Token == refreshToken)
                   .Select(t => t.UserId)
                   .SingleOrDefaultAsync(cancellationToken)
               ?? throw new KnownException("无效的令牌");
    }
}