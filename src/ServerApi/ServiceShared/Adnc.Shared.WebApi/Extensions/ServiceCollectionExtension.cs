using Adnc.Shared.WebApi.Registrar;

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
        var webApiRegistarType = serviceInfo.StartAssembly.ExportedTypes.FirstOrDefault(m => m.IsAssignableTo(typeof(IDependencyRegistrar)) && m.IsAssignableTo(typeof(AbstractWebApiDependencyRegistrar)) && m.IsNotAbstractClass(true));
        if (webApiRegistarType is null)
            throw new NullReferenceException(nameof(IDependencyRegistrar));

        if (Activator.CreateInstance(webApiRegistarType, services) is not IDependencyRegistrar webapiRegistar)
            throw new NullReferenceException(nameof(webapiRegistar));

        webapiRegistar.AddAdnc();

        return services;
    }
}