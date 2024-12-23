using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity
{
    public interface IDepartmentRepository : IRepository<Department, DeptId>;

    public class DepartmentRepository(ApplicationDbContext context) : RepositoryBase<Department, DeptId, ApplicationDbContext>(context), IDepartmentRepository
    {
    }
}
