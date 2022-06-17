using Adnc.Infra.Core.Configuration;

namespace Microsoft.Extensions.Configuration;

public static partial class ConfigurationExtensions
{
    /// <summary>
    /// 服务注册类型
    /// </summary>
    /// <param name="serviceInfo"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetRegisteredType(this IConfiguration configuration) => configuration.GetValue("RegisteredType", "direct");

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
    public static IConfigurationSection GetConsulSection(this IConfiguration configuration) => configuration.GetSection(ConsulConfig.Name);

    /// <summary>
    /// 获取Rabitmq配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetRabbitMqSection(this IConfiguration configuration) => configuration.GetSection(RabbitMqConfig.Name);

    /// <summary>
    /// 获取Redis配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetRedisSection(this IConfiguration configuration) => configuration.GetSection(RedisConfig.Name);

    /// <summary>
    /// 获取Mysql配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetMysqlSection(this IConfiguration configuration) => configuration.GetSection(MysqlConfig.Name);

    /// <summary>
    /// 获取Monogo配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetMongoDbSection(this IConfiguration configuration) => configuration.GetSection(MongoConfig.Name);

    /// <summary>
    /// 获取JWT配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetJWTSection(this IConfiguration configuration) => configuration.GetSection(JwtConfig.Name);

    /// <summary>
    /// 获取线程池配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetThreadPoolSettingsSection(this IConfiguration configuration) => configuration.GetSection("ThreadPoolSettings");

    /// <summary>
    /// 获取Kestrel配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetKestrelSection(this IConfiguration configuration) => configuration.GetSection(KestrelConfig.Name);

    /// <summary>
    /// 获取RpcAddressInfo配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetRpcAddressInfoSection(this IConfiguration configuration) => configuration.GetSection("RpcAddressInfo");

    /// <summary>
    /// 获取RpcPartners配置
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfigurationSection GetRpcPartnersSection(this IConfiguration configuration) => configuration.GetSection("RpcPartners");
}