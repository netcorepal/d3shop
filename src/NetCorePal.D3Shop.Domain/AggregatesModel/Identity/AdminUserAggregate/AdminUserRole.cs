using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public class AdminUserRole
    {
        protected AdminUserRole() { }

        public AdminUserId AdminUserId { get; private set; } = default!;
        public RoleId RoleId { get; private set; } = default!;
        public string RoleName { get; private set; } = string.Empty;

        public AdminUserRole(RoleId roleId, string roleName)
        {
            RoleId = roleId;
            RoleName = roleName;
        }

        public void UpdateRoleInfo(string roleName)
        {
            RoleName = roleName;
        }
    }
}
