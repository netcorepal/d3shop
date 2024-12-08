namespace NetCorePal.D3Shop.Admin.Shared.Permission;

public static class PermissionCodes
{
    #region AdminUserManagement

    public const string AdminUserManagement = nameof(AdminUserManagement);
    public const string AdminUserCreate = nameof(AdminUserCreate);
    public const string AdminUserEdit = nameof(AdminUserEdit);
    public const string AdminUserUpdateRoles = nameof(AdminUserUpdateRoles);
    public const string AdminUserSetPermissions = nameof(AdminUserSetPermissions);
    public const string AdminUserView = nameof(AdminUserView);
    public const string AdminUserUpdatePassword = nameof(AdminUserUpdatePassword);
    public const string AdminUserDelete = nameof(AdminUserDelete);

    #endregion

    #region RoleManagement

    public const string RoleManagement = nameof(RoleManagement);
    public const string RoleCreate = nameof(RoleCreate);
    public const string RoleEdit = nameof(RoleEdit);
    public const string RoleUpdatePermissions = nameof(RoleUpdatePermissions);
    public const string RoleDelete = nameof(RoleDelete);
    public const string RoleView = nameof(RoleView);

    #endregion
}