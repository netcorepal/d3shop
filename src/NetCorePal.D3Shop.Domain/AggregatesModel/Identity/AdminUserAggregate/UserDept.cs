using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public class UserDept
    {
        protected UserDept() { }

        public AdminUserId AdminUserId { get; private set; } = default!;
        public DeptId DeptId { get; private set; } = default!;
        public string DeptName { get; private set; } = string.Empty;

        public string DeptCode { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

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
