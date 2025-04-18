namespace NetCorePal.D3Shop.Admin.Shared.Permission;

public static class PermissionCodes
{


    #region Welcome
    public const string WelcomeManagement = nameof(WelcomeManagement);
    #endregion

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

    #region Menu
    public const string MenuManagement = nameof(MenuManagement);
    public const string MenuCreate = nameof(MenuCreate);
    public const string MenuEdit = nameof(MenuEdit);
    public const string MenuUpdatePermissions = nameof(MenuUpdatePermissions);
    public const string MenuDelete = nameof(MenuDelete);
    public const string MenuView = nameof(MenuView);

    #endregion

    #region DepartmentManagement
    public const string DepartmentManagement = nameof(DepartmentManagement);
    public const string DepartmentCreate = nameof(DepartmentCreate);
    public const string DepartmentEdit = nameof(DepartmentEdit);
    public const string DepartmentView = nameof(DepartmentView);
    public const string DepartmentDelete = nameof(DepartmentDelete);

    #endregion
}