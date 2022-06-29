namespace Microsoft.Extensions.Configuration;

public static partial class ConfigurationExtensions
{
    /// <summary>
    /// 获取服务注册类型
    /// </summary>
    /// <param name="serviceInfo"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetRegisteredType(this IConfiguration configuration) => configuration.GetValue("RegisteredType", "direct");
}