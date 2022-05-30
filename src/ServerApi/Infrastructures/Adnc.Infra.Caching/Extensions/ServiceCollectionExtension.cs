using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.Core.Serialization;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Caching.StackExchange;
using Adnc.Infra.Core.Interceptor;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraCaching(this IServiceCollection services, IConfigurationSection redisSection)
    {
        var cacheOptions = redisSection.Get<CacheOptions>();
        return AddAdncInfraCaching(services, cacheOptions);
    }

    public static IServiceCollection AddAdncInfraCaching(this IServiceCollection services, CacheOptions cacheOptions)
    {
        if (services.HasRegistered(nameof(AddAdncInfraCaching)))
            return services;

        services.AddSingleton(cacheOptions);
        services.AddSingleton<IRedisDatabaseProvider, DefaultDatabaseProvider>();
        services.AddSingleton<ICachingKeyGenerator, DefaultCachingKeyGenerator>();
        services.AddSingleton<DefaultRedisProvider>();
        services.AddSingleton<IRedisProvider>(x => x.GetRequiredService<DefaultRedisProvider>());
        services.AddSingleton<IDistributedLocker>(x => x.GetRequiredService<DefaultRedisProvider>());
        services.AddSingleton<ICacheProvider>(x => x.GetRequiredService<DefaultRedisProvider>());
        services.AddScoped<CachingInterceptor>();
        services.AddScoped<CachingAsyncInterceptor>();

        var serviceType = typeof(ICachingSerializer);
        var implementations = serviceType.Assembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implementations.ForEach(implementationType => services.AddSingleton(serviceType, implementationType));

        return services;
    }
}