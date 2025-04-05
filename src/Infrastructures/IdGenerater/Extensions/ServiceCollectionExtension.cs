using Adnc.Infra.IdGenerater.Yitter;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraYitterIdGenerater(this IServiceCollection services, IConfigurationSection redisSection, string name, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(redisSection, nameof(redisSection));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        if (services.HasRegistered(nameof(AddAdncInfraYitterIdGenerater)))
        {
            return services;
        }

        //var workerNode = Activator.CreateInstance(typeof(WorkerNode), services) as WorkerNode;
        return services
            .AddAdncInfraRedis(redisSection, serviceLifetime)
            .AddSingleton(provider => ActivatorUtilities.CreateInstance<WorkerNode>(provider, name))
            .AddHostedService<WorkerNodeHostedService>();
    }
}
