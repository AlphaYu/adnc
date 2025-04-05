namespace Adnc.Shared;

public static class GeneralConsts
{
    public const string LinkChar = ":";

    public const int OneYear = 365 * 24 * 60 * 60;
    public const int OneMonth = 30 * 24 * 60 * 60;
    public const int OneWeek = 7 * 24 * 60 * 60;
    public const int OneDay = 24 * 60 * 60;
    public const int OneHour = 60 * 60;
    public const int OneMinute = 60;
    public const string UserValidatedInfoKeyPrefix = "admin:users:validatedinfo";
    public const string RoleMenuCodesCacheKey = "admin:role:menus:codes";
}

public static class NodeConsts
{
    public const string ConfigurationType = "ConfigurationType";
    public const string RegisterType = "RegisterType";
    public const string Kestrel = "Kestrel";
    public const string ThreadPoolSettings = "ThreadPoolSettings";
    public const string Redis = "Redis";
    public const string ConnectionString = "ConnectionString";
    public const string Mysql = "Mysql";
    public const string Mysql_ConnectionString = "Mysql:ConnectionString";
    public const string Mysql_ServerVersion = "Mysql:ServerVersion";
    public const string Mysql_ServerType = "Mysql:ServerType";
    public const string SqlServer = "SqlServer";
    public const string SqlServer_ConnectionString = "SqlServer:ConnectionString";
    public const string SysLogDb = "SysLogDb";
    public const string SysLogDb_DbType = "SysLogDb:DbType";
    public const string SysLogDb_ConnectionString = "SysLogDb:ConnectionString";
    public const string Caching = "Caching";
    public const string Consul = "Consul";
    public const string Nacos = "Nacos";
    public const string RabbitMq = "RabbitMq";
    public const string MongoDb = "MongoDb";
    public const string MongoDb_ConnectionString = "MongoDb:ConnectionString";
    public const string RpcInfo = "RpcInfo";
    public const string JWT = "JWT";
    public const string Basic = "Basic";
    public const string Logging = "Logging";
    public const string Logging_LogContainer = "Logging:LogContainer";
    public const string SwaggerUI = "SwaggerUI";
    public const string SwaggerUI_Enable = "SwaggerUI:Enable:";
    public const string Metrics = "Metrics";
    public const string Metrics_Enable = "Metrics:Enable";
}

public static class RegisteredTypeConsts
{
    public const string Direct = "Direct";
    public const string Consul = "Consul";
    public const string Nacos = "Nacos";
    public const string CoreDns = "CoreDns";
}

public static class ConfigurationTypeConsts
{
    public const string File = "File";
    public const string Consul = "Consul";
    public const string Nacos = "Nacos";
    public const string Etcd = "Etcd";
}
