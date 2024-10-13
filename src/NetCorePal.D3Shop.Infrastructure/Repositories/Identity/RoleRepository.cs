using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity
{
    public interface IRoleRepository : IRepository<Role, RoleId>;

    public class RoleRepository(ApplicationDbContext context): RepositoryBase<Role, RoleId, ApplicationDbContext>(context), IRoleRepository
    {
    }
}
