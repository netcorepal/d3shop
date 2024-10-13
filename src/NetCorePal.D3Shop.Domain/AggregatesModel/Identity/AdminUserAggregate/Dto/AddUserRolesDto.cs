using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto
{
    public class AddUserRoleDto
    {
        public RoleId RoleId { get; set; } = default!;
        public string RoleName { get; set; } = string.Empty;
        public IEnumerable<AddUserPermissionDto> Permissions { get; set; } = [];
    }
}
