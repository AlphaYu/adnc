namespace Microsoft.Extensions.Configuration;

public static partial class ConfigurationExtensions
{
    /// <summary>
    /// 获取SSOAuthentication是否开启
    /// </summary>
    /// <param name="serviceInfo"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static bool IsSSOAuthentication(this IConfiguration configuration) => configuration.GetValue("SSOAuthentication", false);

    /// <summary>
    /// 获取跨域配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetAllowCorsHosts(this IConfiguration configuration) => configuration.GetValue("CorsHosts", string.Empty);

    /// <summary>
    /// 获取Consul配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetConsulSection(this IConfiguration configuration) => configuration.GetSection("Consul");

    /// <summary>
    /// 获取Rabitmq配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetRabbitMqSection(this IConfiguration configuration) => configuration.GetSection("RabbitMq");

    /// <summary>
    /// 获取Redis配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetRedisSection(this IConfiguration configuration) => configuration.GetSection("Redis");

    /// <summary>
    /// 获取Mysql配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetMysqlSection(this IConfiguration configuration) => configuration.GetSection("Mysql");

    /// <summary>
    /// 获取Monogo配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetMongoDbSection(this IConfiguration configuration) => configuration.GetSection("MongoDb");

    /// <summary>
    /// 获取JWT配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetJWTSection(this IConfiguration configuration) => configuration.GetSection("JWT");

    /// <summary>
    /// 获取线程池配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetThreadPoolSettingsSection(this IConfiguration configuration) => configuration.GetSection("ThreadPoolSettings");

    /// <summary>
    /// 获取Hangfire配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetHangfireSection(this IConfiguration configuration) => configuration.GetSection("Hangfire");
}