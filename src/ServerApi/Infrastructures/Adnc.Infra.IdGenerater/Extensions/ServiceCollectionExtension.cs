using Adnc.Infra.IdGenerater.Yitter;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraYitterIdGenerater(this IServiceCollection services, IConfigurationSection redisSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraYitterIdGenerater)))
            return services;

        return services
            .AddAdncInfraRedis(redisSection)
            .AddSingleton<WorkerNode>()
            .AddHostedService<WorkerNodeHostedService>();
    }
}