namespace Adnc.Demo.Shared.Const.Permissions.Maint;

public static class PermissionConsts
{
    public static class Cfg
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

    public static class Log
    {
        public const string GetListForOperationLog = "ops-log";
        public const string GetListForLogingLog = "login-log";
    }
}