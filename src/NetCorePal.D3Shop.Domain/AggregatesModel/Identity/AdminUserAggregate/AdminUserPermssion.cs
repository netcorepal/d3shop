using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate
{
    public class AdminUserPermission
    {
        protected AdminUserPermission()
        {
        }

        public AdminUserId AdminUserId { get; private set; } = default!;
        public string PermissionCode { get; private set; } = string.Empty;
        public string PermissionRemark { get; private set; } = string.Empty;
        public List<RoleId> SourceRoleIds { get; } = [];

        public AdminUserPermission(string permissionCode, string permissionRemark, RoleId? sourceRoleId = null)
        {
            PermissionCode = permissionCode;
            PermissionRemark = permissionRemark;
            if (sourceRoleId is not null)
            {
                SourceRoleIds.Add(sourceRoleId);
            }
        }

        public void AddSourceRoleId(RoleId roleId)
        {
            if (SourceRoleIds.Contains(roleId)) return;
            SourceRoleIds.Add(roleId);
        }
    }
}