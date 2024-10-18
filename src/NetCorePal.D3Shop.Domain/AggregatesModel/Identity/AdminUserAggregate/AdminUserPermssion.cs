using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public class AdminUserPermission
    {
        protected AdminUserPermission() { }

        public AdminUserId AdminUserId { get; internal set; } = default!;
        public string PermissionCode { get; private set; } = string.Empty;
        public string PermissionRemark { get; private set; } = string.Empty;
        public List<RoleId> SourceRoleIds { get; private set; } = [];

        public AdminUserPermission(string permissionCode, string permissionRemark)
        {
            PermissionCode = permissionCode;
            PermissionRemark = permissionRemark;
        }

        public void AddSourceRoleId(RoleId roleId)
        {
            if (SourceRoleIds.Contains(roleId)) return;
            SourceRoleIds.Add(roleId);
        }

        public void RemoveSourceRoleId(RoleId roleId)
        {
            SourceRoleIds!.Remove(roleId);
        }
    }
}
