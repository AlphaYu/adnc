using Adnc.Shared.Domain;

namespace Microsoft.Extensions.DependencyInjection;

public static class AdncDomainServiceCollectionExtension
{
    public static IServiceCollection AddAdncDomainManagers(this IServiceCollection services, Assembly entitiesAssemblieToScan)
    {
        services.Scan(scan => scan.FromAssemblies(entitiesAssemblieToScan)
                        .AddClasses(c => c.AssignableTo<IDomainService>())
                        .AsSelf()
                        .WithScopedLifetime());
        return services;
    }
}