namespace Adnc.Demo.Shared.Const.Permissions.Maint;

public static class PermissionConsts
{
    public static class Cfg
    {
        public const string Create = "cfgAdd";
        public const string Update = "cfgEdit";
        public const string Delete = "cfgDelete";
        public const string GetList = "cfgList";
    }

    public static class Dict
    {
        public const string Create = "dictAdd";
        public const string Update = "dictEdit";
        public const string Delete = "dictDelete";
        public const string GetList = "dictList";
        public const string Get = "dictGet";
    }

    public static class Log
    {
        public const string GetListForOperationLog = "opsLog";
        public const string GetListForLogingLog = "loginLog";
        public const string GetListForNLog = "nlogLog";
    }
}