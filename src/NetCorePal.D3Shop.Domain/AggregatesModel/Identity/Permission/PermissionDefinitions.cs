namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;

public static class PermissionDefinitions
{
    #region AdminUserManagement
    public const string AdminUserCreate = nameof(AdminUserCreate);
    public const string AdminUserEdit = nameof(AdminUserEdit);
    public const string AdminUserUpdateRoles = nameof(AdminUserUpdateRoles);
    public const string AdminUserView = nameof(AdminUserView);
    public const string AdminUserUpdatePassword = nameof(AdminUserUpdatePassword);
    public const string AdminUserDelete = nameof(AdminUserDelete);
    #endregion

    #region RoleManagement
    public const string RoleCreate = nameof(RoleCreate);
    public const string RoleEdit = nameof(RoleEdit);
    public const string RoleUpdatePermissions = nameof(RoleUpdatePermissions);
    public const string RoleDelete = nameof(RoleDelete);
    public const string RoleView = nameof(RoleView);
    #endregion

}