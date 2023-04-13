namespace Adnc.Demo.Shared.Const.Permissions.Usr;

public static class PermissionConsts
{
    public static class User
    {
        public const string Create = "userAdd";
        public const string Update = "userEdit";
        public const string Delete = "userDelete";
        public const string SetRole = "userSetRole";
        public const string GetList = "userList";
        public const string ChangeStatus = "userFreeze";
    }

    public static class Dept
    {
        public const string Create = "deptAdd";
        public const string Update = "deptEdit";
        public const string Delete = "deptDelete";
        public const string GetList = "deptList";
    }

    public static class Menu
    {
        public const string Create = "menuAdd";
        public const string Update = "menuEdit";
        public const string Delete = "menuDelete";
        public const string GetList = "menuList";
    }

    public static class Role
    {
        public const string Create = "roleAdd";
        public const string Update = "roleEdit";
        public const string Delete = "roleDelete";
        public const string GetList = "roleList";
        public const string SetPermissons = "roleSetAuthority";
    }
}