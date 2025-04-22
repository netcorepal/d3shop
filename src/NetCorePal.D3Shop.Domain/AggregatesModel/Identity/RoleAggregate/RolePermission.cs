using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate
{
    public class RolePermission
    {
        protected RolePermission()
        {
        }

        public RoleId RoleId { get; internal set; } = default!;
        public string PermissionCode { get; private set; } = string.Empty;


        //public MenuId MenuId { get; internal set; } = default!;
        public MenuId MenuId { get; internal set; } = default!;

        public RolePermission(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        public RolePermission(string permissionCode, MenuId menuId)
        {
            PermissionCode = permissionCode;
            MenuId = menuId;
        }
    }
}