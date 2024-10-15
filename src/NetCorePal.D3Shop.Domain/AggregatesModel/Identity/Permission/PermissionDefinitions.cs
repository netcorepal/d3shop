namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;

public static class PermissionDefinitions
{
    public static class AdminUserManagement
    {
        public const string Create = "AdminUser_CREATE";
        public const string Edit = "AdminUser_Edit";
        public const string Delete = "AdminUser_Delete";
        public const string View = "AdminUser_View";
    }

    public static class RoleManagement
    {
        public const string Create = "Role_Create";
        public const string Edit = "Role_Create";
        public const string Delete = "Role_Create";
        public const string View = "Role_Create";
    }

}