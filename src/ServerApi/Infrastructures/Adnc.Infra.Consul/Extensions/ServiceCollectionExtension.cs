namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraConsulServiceCollectionExtension
{
    public static IServiceCollection AddAdncConsul(this IServiceCollection services, IConfigurationSection consulSection)
    {
        services.AddScoped<ITokenGenerator, DefaultTokenGenerator>();
        services.AddScoped<SimpleDiscoveryDelegatingHandler>();
        services.AddScoped<ConsulDiscoverDelegatingHandler>();
        services.AddSingleton(x => new ConsulClient(x => x.Address = new Uri(consulSection.Get<ConsulConfig>().ConsulUrl)));
        return services;
    }
}