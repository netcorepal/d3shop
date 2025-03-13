using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin
{
    public interface IDepartmentRepository : IRepository<Department, DeptId>;

    public class DepartmentRepository(ApplicationDbContext context) : RepositoryBase<Department, DeptId, ApplicationDbContext>(context), IDepartmentRepository
    {
    }
}
