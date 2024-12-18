using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public class UserDept
    {
        protected UserDept() { }

        public AdminUserId AdminUserId { get; private set; } = default!;
        public DeptId DeptId { get; private set; } = default!;
        public string DeptName { get; private set; } = string.Empty;

        public UserDept(DeptId deptId, string deptName)
        {
            DeptId = deptId;
            DeptName = deptName;
        }

        public void UpdateDeptInfo(string deptName)
        {
            DeptName = deptName;
        }
    }
}
