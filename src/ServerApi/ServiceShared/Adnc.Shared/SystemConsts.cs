namespace Adnc.Shared
{
    public static class GeneralConsts
    {
        public const string LinkChar = ":";

        public const int OneYear = 365 * 24 * 60 * 60;
        public const int OneMonth = 30 * 24 * 60 * 60;
        public const int OneWeek = 7 * 24 * 60 * 60;
        public const int OneDay = 24 * 60 * 60;
        public const int OneHour = 60 * 60;
        public const int OneMinute = 60;
    }

    public static class NodeConsts
    {
        public const string RegisteredType = "RegisteredType";
        public const string Kestrel = "Kestrel";
        public const string ThreadPoolSettings = "ThreadPoolSettings";
        public const string Redis = "Redis";
        public const string Mysql = "Mysql";
        public const string Mysql_ConnectionString = "Mysql:ConnectionString";
        public const string Caching = "Caching";
        public const string Consul = "Consul";
        public const string RabbitMq = "RabbitMq";
        public const string MongoDb = "MongoDb";
        public const string MongoDb_ConnectionString = "MongoDb:ConnectionString";
        public const string RpcAddressInfo = "RpcAddressInfo";
        public const string RpcPartners = "RpcPartners";
        public const string JWT = "JWT";
        public const string Logging = "Logging";
        public const string Logging_LogContainer = "Logging:LogContainer";
        public const string SwaggerUI = "SwaggerUI";
        public const string SwaggerUI_Enable = "SwaggerUI:Enable:";
        public const string Metrics = "Metrics";
        public const string Metrics_Enable = "Metrics:Enable";
    }

    public static class RegisteredTypeConsts
    {
        public const string Direct = "direct";
        public const string Consul = "consul";
        public const string Nacos = "nacos";
        public const string ClusterIP = "clusterip";
    }
}
