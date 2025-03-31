using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;

public interface IClientUserRepository : IRepository<ClientUser, ClientUserId>
{
    public Task<ClientUser?> GetIncludeRefreshTokensAsync(ClientUserId id, CancellationToken cancellationToken);
}

public class ClientUserRepository(ApplicationDbContext context)
    : RepositoryBase<ClientUser, ClientUserId, ApplicationDbContext>(context), IClientUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<ClientUser?> GetIncludeRefreshTokensAsync(ClientUserId id, CancellationToken cancellationToken)
    {
        return _context.ClientUsers.Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}