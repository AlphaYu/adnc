namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///  统一注册Adnc.WebApi通用服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="startupAssembly"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public static IServiceCollection AddAdnc(this IServiceCollection services, IServiceInfo serviceInfo)
    {
        if (serviceInfo?.StartAssembly is null)
            throw new ArgumentNullException(nameof(serviceInfo));

        var webApiRegistarType = serviceInfo.StartAssembly.ExportedTypes.FirstOrDefault(m => m.IsAssignableTo(typeof(IDependencyRegistrar)) && m.IsNotAbstractClass(true));
        if (webApiRegistarType is null)
            throw new NullReferenceException(nameof(IDependencyRegistrar));

        var webapiRegistar = Activator.CreateInstance(webApiRegistarType, services)  as IDependencyRegistrar;
        webapiRegistar.AddAdnc();

        return services;
    }
}