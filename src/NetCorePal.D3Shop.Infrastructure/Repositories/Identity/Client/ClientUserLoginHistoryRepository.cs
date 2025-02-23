using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserLoginHistoryAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;

public interface IClientUserLoginHistoryRepository : IRepository<ClientUserLoginHistory, ClientUserLoginHistoryId>;

public class ClientUserLoginHistoryRepository(ApplicationDbContext context)
    : RepositoryBase<ClientUserLoginHistory, ClientUserLoginHistoryId, ApplicationDbContext>(context),
        IClientUserLoginHistoryRepository;