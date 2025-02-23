using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;

public interface IClientUserRepository : IRepository<ClientUser, ClientUserId>;

public class ClientUserRepository(ApplicationDbContext context)
    : RepositoryBase<ClientUser, ClientUserId, ApplicationDbContext>(context), IClientUserRepository;