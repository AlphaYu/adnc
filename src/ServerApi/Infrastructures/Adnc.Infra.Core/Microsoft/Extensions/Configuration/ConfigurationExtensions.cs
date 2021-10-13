namespace Microsoft.Extensions.Configuration
{
    public static partial class ConfigurationExtensions
    {
        /// <summary>
        /// 获取SSOAuthentication是否开启
        /// </summary>
        /// <param name="serviceInfo"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsSSOAuthentication(this IConfiguration configuration)
        {
            return configuration.GetValue("SSOAuthentication", false);
        }

        /// <summary>
        /// 获取跨域配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string GetAllowCorsHosts(this IConfiguration configuration)
        {
            return configuration.GetValue("CorsHosts", string.Empty);
        }

        public static IConfigurationSection GetConsulSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Consul");
        }

        public static IConfigurationSection GetRabbitMqSection(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq");
        }

        public static IConfigurationSection GetRedisSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Redis");
        }

        public static IConfigurationSection GetMysqlSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Mysql");
        }

        public static IConfigurationSection GetMongoDbSection(this IConfiguration configuration)
        {
            return configuration.GetSection("MongoDb");
        }

        public static IConfigurationSection GetJWTSection(this IConfiguration configuration)
        {
            return configuration.GetSection("JWT");
        }

        public static IConfigurationSection GetThreadPoolSettingsSection(this IConfiguration configuration)
        {
            return configuration.GetSection("ThreadPoolSettings");
        }

        public static IConfigurationSection GetHangfireSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Hangfire");
        }
    }
}