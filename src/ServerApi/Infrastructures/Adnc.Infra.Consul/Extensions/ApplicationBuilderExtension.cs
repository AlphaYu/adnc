using Adnc.Infra.Consul.Registrar;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderConsulExtension
{
    public static void RegisterToConsul(this IApplicationBuilder app)
    {
        var kestrelConfig = app.ApplicationServices.GetRequiredService<IOptions<KestrelConfig>>()?.Value;
        if (kestrelConfig is null)
            throw new NotImplementedException(nameof(kestrelConfig));

        var registration = app.ApplicationServices.GetRequiredService<ConsulRegistration>();
        var ipAddresses = registration.GetLocalIpAddress("InterNetwork");
        if (ipAddresses.IsNullOrEmpty())
            throw new NotImplementedException(nameof(kestrelConfig));

        var defaultEnpoint = kestrelConfig.Endpoints.FirstOrDefault(x => x.Key.EqualsIgnoreCase("default")).Value;
        if (defaultEnpoint is null || defaultEnpoint.Url.IsNullOrWhiteSpace())
            throw new NotImplementedException(nameof(kestrelConfig));

        var serviceAddress = new Uri(defaultEnpoint.Url);
        if (serviceAddress.Host == "0.0.0.0")
            serviceAddress = new Uri($"{serviceAddress.Scheme}://{ipAddresses.FirstOrDefault()}:{serviceAddress.Port}");

        registration.Register(serviceAddress);
    }

    public static void RegisterToConsul(this IApplicationBuilder app, Uri serviceAddress)
    {
        if (serviceAddress is null)
            throw new ArgumentNullException(nameof(serviceAddress));

        var registration = app.ApplicationServices.GetRequiredService<ConsulRegistration>();
        registration.Register(serviceAddress);
    }

    public static void RegisterToConsul(this IApplicationBuilder app, AgentServiceRegistration instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        var registration = app.ApplicationServices.GetRequiredService<ConsulRegistration>();
        registration.Register(instance);
    }
}