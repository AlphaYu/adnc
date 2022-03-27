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
        //注册Consul服务
        var configuration = services.GetConfiguration();
        var consulUrl = configuration.GetConsulSection().Get<ConsulConfig>().ConsulUrl;
        builder.RegisterModuleIfNotRegistered(new AdncInfraConsulModule(consulUrl));

        //注册Application服务
        var serviceInfo = services.GetServiceInfo();
        var appAssemblyPath = serviceInfo.AssemblyLocation.Replace("WebApi", "Application");
        var appAssembly = Assembly.LoadFrom(appAssemblyPath);
        var appModelType = appAssembly.GetTypes()
                                                      .FirstOrDefault(
                                                        m => m.FullName != null
                                                        && typeof(Autofac.Module).IsAssignableFrom(m)
                                                        && !m.IsAbstract
                                                       );
        builder.RegisterModuleIfNotRegistered(Activator.CreateInstance(appModelType, configuration, serviceInfo) as Autofac.Module);

        completedExecute?.Invoke(builder);
        return builder;
    }
}