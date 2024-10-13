namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public class RolePermission
    {
        protected RolePermission() { }
        public RoleId RoleId { get; private set; } = default!;
        public string PermissionCode { get; private set; } = string.Empty;
        public string PermissionRemark { get; private set; } = string.Empty;

        public RolePermission(RoleId roleId, string permissionCode, string permissionRemark)
        {
            RoleId = roleId;
            PermissionCode = permissionCode;
            PermissionRemark = permissionRemark;
        }
    }
}
