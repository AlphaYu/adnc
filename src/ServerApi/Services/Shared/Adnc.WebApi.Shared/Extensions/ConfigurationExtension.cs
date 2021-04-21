using Adnc.Infr.Consul;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtension
    {
        /// <summary>
        /// 获取consul配置
        /// </summary>
        /// <param name="serviceInfo"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ConsulConfig GetConsulConfig(this IConfiguration configuration)
        {
            return configuration.GetConsulSection().Get<ConsulConfig>();
        }

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

        public static IConfiguration GetConsulSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Consul");
        }

        public static IConfiguration GetRabbitMqSection(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq");
        }

        public static IConfiguration GetRedisSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Redis");
        }


        public static IConfiguration GetMysqlSection(this IConfiguration configuration)
        {
            return configuration.GetSection("Mysql");
        }

        public static IConfiguration GetMongoDbSection(this IConfiguration configuration)
        {
            return configuration.GetSection("MongoDb");
        }

        public static IConfiguration GetJWTSection(this IConfiguration configuration)
        {
            return configuration.GetSection("JWT");
        }
    }
}
