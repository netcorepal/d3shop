using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.Extensions.Repository;
using NetCorePal.Extensions.Repository.EntityFrameworkCore;

namespace NetCorePal.D3Shop.Infrastructure.Repositories.Identity
{
    public interface IAdminUserRepository : IRepository<AdminUser, AdminUserId>;

    public class AdminUserRepository(ApplicationDbContext context) : RepositoryBase<AdminUser, AdminUserId, ApplicationDbContext>(context), IAdminUserRepository
    {
    }
}
