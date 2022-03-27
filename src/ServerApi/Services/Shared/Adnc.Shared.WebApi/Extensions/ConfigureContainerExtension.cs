namespace Autofac;

public static class ConfigureContainerExtension
{
    /// <summary>
    /// 统一注册Adnc.WebApi通用模块
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <param name="serverInfo"></param>
    /// <param name="completedExecute"></param>
    /// <returns></returns>
    public static ContainerBuilder RegisterAdncModules(this ContainerBuilder builder, IServiceCollection services, Action<ContainerBuilder> completedExecute = null)
    {
        LoadDepends(builder, services);
        completedExecute?.Invoke(builder);
        return builder;
    }

    /// <summary>
    /// 注册依赖模块
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="services"></param>
    internal static void LoadDepends(ContainerBuilder builder, IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        var serviceInfo = services.GetServiceInfo();
        var appAssemblyPath = serviceInfo.AssemblyLocation.Replace(".WebApi.dll", ".Application.dll");
        var appAssembly = Assembly.LoadFrom(appAssemblyPath);
        //var appAssemblyName = serviceInfo.AssemblyFullName.Replace("WebApi", "Application");
        //var appAssembly = Assembly.Load(appAssemblyName);
        var appModelType = appAssembly.GetTypes()
                                                      .FirstOrDefault(
                                                        m => m.FullName != null
                                                        && typeof(Autofac.Module).IsAssignableFrom(m)
                                                        && !m.IsAbstract
                                                       );
        var adncApplicationModule = Activator.CreateInstance(appModelType, configuration, serviceInfo) as Autofac.Module;
        builder.RegisterModuleIfNotRegistered(adncApplicationModule);
    }
}