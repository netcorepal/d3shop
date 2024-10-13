using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate.Dto
{
    public class AddRolePermissionDto
    {
        public string PermissionCode { get; private set; } = string.Empty;
        public string PermissionRemark { get; private set; } = string.Empty;
    }
}
