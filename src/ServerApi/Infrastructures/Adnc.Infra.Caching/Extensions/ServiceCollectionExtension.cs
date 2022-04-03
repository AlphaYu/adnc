using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.Core.Serialization;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Caching.StackExchange;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncCaching(this IServiceCollection services, IConfigurationSection redisSection)
    {
        var cacheOptions = redisSection.Get<CacheOptions>();
        services.AddSingleton(cacheOptions);
        services.AddSingleton<IRedisDatabaseProvider, DefaultDatabaseProvider>();
        services.AddSingleton<DefaultRedisProvider>();
        services.AddSingleton<IRedisProvider>(x => x.GetRequiredService<DefaultRedisProvider>());
        services.AddSingleton<IDistributedLocker>(x => x.GetRequiredService<DefaultRedisProvider>());
        services.AddSingleton<ICacheProvider>(x => x.GetRequiredService<DefaultRedisProvider>());
        services.AddScoped<CachingInterceptor>();
        services.AddScoped<CachingAsyncInterceptor>();
        services.Scan(scan => scan.FromAssemblyOf<ICachingSerializer>()
        .AddClasses(c => c.AssignableTo<ICachingSerializer>())
        .AsImplementedInterfaces()
        .WithSingletonLifetime());

        return services;
    }
}