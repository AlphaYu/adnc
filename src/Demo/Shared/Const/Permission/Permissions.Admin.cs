namespace Adnc.Demo.Shared.Const.Permissions.Admin;

public static class PermissionConsts
{
    public static class User
    {
        public const string Create = "user-create";
        public const string Update = "user-update";
        public const string Delete = "user-delete";
        public const string SetRole = "user-setrole";
        public const string GetList = "user-list";
        public const string ResetPassword = "user-reset-password";
    }

    public static class Org
    {
        public const string Create = "org-create";
        public const string Update = "org-update";
        public const string Delete = "org-delete";
        public const string GetList = "org-list";
    }

    public static class Menu
    {
        public const string Create = "menu-create";
        public const string Update = "menu-update";
        public const string Delete = "menu-delete";
        public const string GetList = "menu-list";
    }

    public static class Role
    {
        public const string Create = "role-create";
        public const string Update = "role-update";
        public const string Delete = "role-delete";
        public const string GetList = "role-list";
        public const string SetPermissons = "role-setperms";
    }

    public static class SysConfig
    {
        public const string Create = "cfg-create";
        public const string Update = "cfg-update";
        public const string Delete = "cfg-delete";
        public const string GetList = "cfg-list";
    }

    public static class Dict
    {
        public const string Create = "dict-create";
        public const string Update = "dict-update";
        public const string Delete = "dict-delete";
        public const string GetList = "dict-list";
        public const string Get = "dict-get";
    }

    public static class DictData
    {
        public const string Create = "dictdata-create";
        public const string Update = "dictdata-update";
        public const string Delete = "dictdata-delete";
        public const string GetList = "dictdata-list";
    }
}