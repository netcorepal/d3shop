using System.Collections.ObjectModel;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission
{
    public record Permission(string Code, string GroupName, string Remark);

    public static class Permissions
    {
        private static readonly Permission[] All =
            [
                #region AdminUserManagement
                new(PermissionDefinitions.AdminUserManagement.Create,
                    PermissionGroup.SystemAccess,
                    "Create AdminUser"),
                new(PermissionDefinitions.AdminUserManagement.Edit,
                    PermissionGroup.SystemAccess,
                    "Update AdminUser"),
                new(PermissionDefinitions.AdminUserManagement.Delete,
                    PermissionGroup.SystemAccess,
                    "Delete AdminUser"),
                new(PermissionDefinitions.AdminUserManagement.View,
                    PermissionGroup.SystemAccess,
                    "Manage AdminUser"),
                #endregion
                
            ];

        public static IReadOnlyList<Permission> AllPermissions { get; } =
            new ReadOnlyCollection<Permission>(All);
    }
}
