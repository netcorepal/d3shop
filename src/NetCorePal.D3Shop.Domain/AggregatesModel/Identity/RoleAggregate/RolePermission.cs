namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public class RolePermission
    {
        protected RolePermission() { }
        public RoleId RoleId { get; internal set; } = default!;
        public string PermissionCode { get; private set; } = string.Empty;
        public string PermissionRemark { get; private set; } = string.Empty;

        public RolePermission(string permissionCode, string permissionRemark)
        {
            PermissionCode = permissionCode;
            PermissionRemark = permissionRemark;
        }
    }
}
