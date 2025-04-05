using Adnc.Infra.Consul.Registrar;

namespace Microsoft.Extensions.Hosting;

public static class ApplicationBuilderConsulExtension
{
    public static IHost RegisterToConsul(this IHost host, string? serviceId, IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(configurationSection, nameof(IConfigurationSection));
        var logger = host.Services.GetRequiredService<ILogger<KestrelOptions>>();

        logger.LogInformation("{serviceId} start register to consul", serviceId);

        var kestrelOptions = configurationSection.Get<KestrelOptions>();

        var registration = ActivatorUtilities.CreateInstance<RegistrationProvider>(host.Services);
        var ipAddresses = registration.GetLocalIpAddress("InterNetwork");
        if (ipAddresses.IsNullOrEmpty())
        {
            throw new NotImplementedException(nameof(KestrelOptions));
        }

        var defaultEnpoint = kestrelOptions?.Endpoints.FirstOrDefault(x => x.Key.EqualsIgnoreCase("default")).Value;
        if (defaultEnpoint is null || defaultEnpoint.Url.IsNullOrWhiteSpace())
        {
            throw new NotImplementedException(nameof(KestrelOptions));
        }

        var serviceAddress = new Uri(defaultEnpoint.Url);
        if (serviceAddress.Host == "0.0.0.0")
        {
            serviceAddress = new Uri($"{serviceAddress.Scheme}://{ipAddresses.FirstOrDefault()}:{serviceAddress.Port}");
        }

        logger.LogInformation("service address {serviceAddress}", serviceAddress);

        registration.Register(serviceAddress, serviceId);
        return host;
    }

    public static IHost RegisterToConsul(this IHost host, Uri serviceAddress, string? serviceId = null)
    {
        ArgumentNullException.ThrowIfNull(serviceAddress);

        var registration = ActivatorUtilities.CreateInstance<RegistrationProvider>(host.Services);
        registration.Register(serviceAddress, serviceId);
        return host;
    }

    public static IHost RegisterToConsul(this IHost host, AgentServiceRegistration instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var registration = ActivatorUtilities.CreateInstance<RegistrationProvider>(host.Services);
        registration.Register(instance);
        return host;
    }
}
