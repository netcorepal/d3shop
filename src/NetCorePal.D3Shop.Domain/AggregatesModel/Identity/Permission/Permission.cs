using System.Collections.ObjectModel;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission
{
    public record Permission(string Feature, string Action, string Group, string Remark, bool IsBasic = false)
    {
        public string Code => CodeFor(Feature, Action);

        public static string CodeFor(string feature, string action)
        {
            return $"Permissions.{feature}.{action}";
        }
    }

    public static class Permissions
    {
        private static readonly Permission[] All =
        [
            new(AppFeature.Users, AppAction.Create, PermissionGroup.SystemAccess, "Create Users"),
            new(AppFeature.Users, AppAction.Update, PermissionGroup.SystemAccess, "Update Users"),
            new(AppFeature.Users, AppAction.Read, PermissionGroup.SystemAccess, "Read Users"),
            new(AppFeature.Users, AppAction.Delete, PermissionGroup.SystemAccess, "Delete Users"),

            new(AppFeature.UserRoles, AppAction.Read, PermissionGroup.SystemAccess, "Read User Roles"),
            new(AppFeature.UserRoles, AppAction.Update, PermissionGroup.SystemAccess, "Update User Roles"),

            new(AppFeature.Roles, AppAction.Read, PermissionGroup.SystemAccess, "Read Roles"),
            new(AppFeature.Roles, AppAction.Create, PermissionGroup.SystemAccess, "Create Roles"),
            new(AppFeature.Roles, AppAction.Update, PermissionGroup.SystemAccess, "Update Roles"),
            new(AppFeature.Roles, AppAction.Delete, PermissionGroup.SystemAccess, "Delete Roles"),

            new(AppFeature.RolePermissions, AppAction.Read, PermissionGroup.SystemAccess, "Read Role Claims/Permissions"),
            new(AppFeature.RolePermissions, AppAction.Update, PermissionGroup.SystemAccess, "Update Role Claims/Permissions"),

            new(AppFeature.Order, AppAction.Read, PermissionGroup.ManagementHierarchy, "Read Orders"),
            new(AppFeature.Order, AppAction.Create, PermissionGroup.ManagementHierarchy, "Create Orders"),
            new(AppFeature.Order, AppAction.Update, PermissionGroup.ManagementHierarchy, "Update Orders"),
            new(AppFeature.Order, AppAction.Delete, PermissionGroup.ManagementHierarchy, "Delete Orders"),

            new(AppFeature.Demo, AppAction.Read, PermissionGroup.ManagementHierarchy, "Read Demos",IsBasic:true),
            new(AppFeature.Demo, AppAction.Create, PermissionGroup.ManagementHierarchy, "Create Demos"),
            new(AppFeature.Demo, AppAction.Update, PermissionGroup.ManagementHierarchy, "Update Demos"),
            new(AppFeature.Demo, AppAction.Delete, PermissionGroup.ManagementHierarchy, "Delete Demos"),

            new(AppFeature.TestAuth, AppAction.Read, PermissionGroup.ManagementHierarchy, "TestAuth Get",IsBasic:true),
            new(AppFeature.TestAuth, AppAction.Create, PermissionGroup.ManagementHierarchy, "TestAuth Post")
        ];

        public static IReadOnlyList<Permission> AdminPermissions { get; } = new ReadOnlyCollection<Permission>(All.Where(p => !p.IsBasic).ToArray());

        public static IReadOnlyList<Permission> BasicPermissions { get; } =
            new ReadOnlyCollection<Permission>(All.Where(p => p.IsBasic).ToArray());

        public static IReadOnlyList<Permission> AllPermissions { get; } =
            new ReadOnlyCollection<Permission>(All);
    }
}
