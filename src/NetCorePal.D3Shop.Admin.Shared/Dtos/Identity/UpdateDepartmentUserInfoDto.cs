using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Dtos.Identity
{
   
    /// <summary>
    /// 更新部门用户信息
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="UserName"></param>
    public record UpdateDepartmentUserInfoDto(AdminUserId UserId, string UserName);
}
