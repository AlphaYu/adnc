using Adnc.Infra.Consul.Registrar;

namespace Microsoft.Extensions.Hosting;

public static class ApplicationBuilderConsulExtension
{
    public static IHost RegisterToConsul(this IHost host, string? serviceId = null)
    {
        var kestrelConfig = host.Services.GetRequiredService<IOptions<KestrelConfig>>().Value;
        if (kestrelConfig is null)
            throw new NotImplementedException(nameof(kestrelConfig));

        var registration = ActivatorUtilities.CreateInstance<RegistrationProvider>(host.Services);
        var ipAddresses = registration.GetLocalIpAddress("InterNetwork");
        if (ipAddresses.IsNullOrEmpty())
            throw new NotImplementedException(nameof(kestrelConfig));

        var defaultEnpoint = kestrelConfig.Endpoints.FirstOrDefault(x => x.Key.EqualsIgnoreCase("default")).Value;
        if (defaultEnpoint is null || defaultEnpoint.Url.IsNullOrWhiteSpace())
            throw new NotImplementedException(nameof(kestrelConfig));

        var serviceAddress = new Uri(defaultEnpoint.Url);
        if (serviceAddress.Host == "0.0.0.0")
            serviceAddress = new Uri($"{serviceAddress.Scheme}://{ipAddresses.FirstOrDefault()}:{serviceAddress.Port}");

        registration.Register(serviceAddress, serviceId);
        return host;
    }

    public static IHost RegisterToConsul(this IHost host, Uri serviceAddress, string? serviceId = null)
    {
        if (serviceAddress is null)
            throw new ArgumentNullException(nameof(serviceAddress));

        var registration = ActivatorUtilities.CreateInstance<RegistrationProvider>(host.Services);
        registration.Register(serviceAddress, serviceId);
        return host;
    }

    public static IHost RegisterToConsul(this IHost host, AgentServiceRegistration instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        var registration = ActivatorUtilities.CreateInstance<RegistrationProvider>(host.Services);
        registration.Register(instance);
        return host;
    }
}