using Adnc.Shared.Consts.AppSettings;
using Adnc.Shared.Consts.RegistrationCenter;

namespace Microsoft.Extensions.Hosting;

public static class HostExtension
{
    /// <summary>
    /// register to (consul/nacos/clusterip...)
    /// </summary>
    public static IHost UseRegistrationCenter(this IHost host)
    {
        var configuration = host.Services.GetService<IConfiguration>();
        var serviceInfo = host.Services.GetService<IServiceInfo>();
        var registeredType = configuration.GetValue(NodeConsts.RegisteredType, "direct");
        switch (registeredType)
        {
            case RegisteredTypeConsts.Consul:
                host.RegisterToConsul(serviceInfo.Id);
                break;
            case RegisteredTypeConsts.Nacos:
                // TODO
                //app.RegisterToNacos(serviceInfo.Id);
                break;
            default:
                break;
        }
        return host;
    }
}
