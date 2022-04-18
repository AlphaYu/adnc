namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// 统一注册Adnc.WebApi通用服务
    /// </summary>
    /// <typeparam name="TPermissionHandler"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="environment"></param>
    /// <param name="serviceInfo"></param>
    /// <param name="completedExecute"></param>
    /// <returns></returns>
    [Obsolete("use : var registrar = services.GetWebApiRegistrar().AddAdncServices();")]
    public static IServiceCollection AddAdncServices(this IServiceCollection services)
    {
        var registrar = services.GetWebApiRegistrar();
        registrar.AddAdnc();
        return services;
    }
}