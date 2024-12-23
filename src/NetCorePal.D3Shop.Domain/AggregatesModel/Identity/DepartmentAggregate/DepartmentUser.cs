using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate
{

    /// <summary>
    /// 部门用户
    /// </summary>
    public class DepartmentUser
    {
        protected DepartmentUser()
        {

        }

        /// <summary>
        /// 部门id
        /// </summary>
        public DeptId DeptId { get; set; } = default!;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; private set; } = string.Empty;

        /// <summary>
        /// 部门id
        /// </summary>
        public AdminUserId UserId { get; set; } = default!;

        public DepartmentUser(string userName, AdminUserId userId)
        {
            UserName = userName;
            UserId = userId;
        }

        public void UpdateUserInfo(string userName)
        {
            UserName = userName;
        }
    }
}
