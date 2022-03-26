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
        var configuration = services.GetConfiguration();
        var serviceInfo = services.GetServiceInfo();

        var consulUrl = configuration.GetConsulSection().Get<ConsulConfig>().ConsulUrl;
        builder.RegisterModuleIfNotRegistered(new AdncInfraConsulModule(consulUrl));

        var applicationAssemblyName = serviceInfo.AssemblyFullName.Replace("WebApi", "Application");
        //var assemblyPath = serviceInfo.AssemblyLocation.Replace("WebApi", "Application");
        var applicationAssembly = Assembly.Load(applicationAssemblyName);
        var applicationModelType = applicationAssembly.GetTypes()
                                                      .FirstOrDefault(
                                                        m => m.FullName != null
                                                        && typeof(Autofac.Module).IsAssignableFrom(m)
                                                        && !m.IsAbstract
                                                       );
        builder.RegisterModuleIfNotRegistered(Activator.CreateInstance(applicationModelType, configuration, serviceInfo) as Autofac.Module);

        completedExecute?.Invoke(builder);

        return builder;
    }
}