using System.Collections.ObjectModel;

namespace NetCorePal.D3Shop.Admin.Shared.Permission
{
    public record Permission(string Code, string GroupName, string Remark);

    public static class Permissions
    {
        private static readonly Permission[] All =
            [
                #region AdminUserManagement
                new (PermissionDefinitions.AdminUserCreate,
                    PermissionGroup.SystemAccess,
                    "创建管理员用户"),
                new (PermissionDefinitions.AdminUserEdit,
                    PermissionGroup.SystemAccess,
                    "更新管理员用户信息"),
                new (PermissionDefinitions.AdminUserDelete,
                    PermissionGroup.SystemAccess,
                    "删除管理员用户"),
                new (PermissionDefinitions.AdminUserView,
                    PermissionGroup.SystemAccess,
                    "查询管理员用户"),
                new (PermissionDefinitions.AdminUserUpdateRoles,
                    PermissionGroup.SystemAccess,
                    "更新管理员用户角色"),
                new (PermissionDefinitions.AdminUserUpdatePassword,
                    PermissionGroup.SystemAccess,
                    "更新管理员用户密码"),
                #endregion

                #region RoleManagement
                new (PermissionDefinitions.RoleCreate,
                    PermissionGroup.SystemAccess,
                    "创建角色"),
                new (PermissionDefinitions.RoleEdit,
                    PermissionGroup.SystemAccess,
                    "更新角色信息"),
                new (PermissionDefinitions.RoleUpdatePermissions,
                    PermissionGroup.SystemAccess,
                    "更新角色权限"),
                new (PermissionDefinitions.RoleView,
                    PermissionGroup.SystemAccess,
                    "查询角色"),
                new (PermissionDefinitions.RoleDelete,
                    PermissionGroup.SystemAccess,
                    "删除角色"),
                #endregion
            ];

        public static IReadOnlyList<Permission> AllPermissions { get; } =
            new ReadOnlyCollection<Permission>(All);
    }
}
