using Adnc.Infra.Redis.Caching.Configurations;
using Adnc.Infra.Redis.Caching.Core.Interceptor;
using Adnc.Infra.Redis.Caching.Interceptor.Castle;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraRedisCaching(this IServiceCollection services, IConfigurationSection redisSection, IConfigurationSection cachingSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraRedisCaching)))
            return services;

        return services
            .AddAdncInfraRedis(redisSection)
            .Configure<CacheOptions>(cachingSection)
            .AddSingleton<ICachingKeyGenerator, DefaultCachingKeyGenerator>()
            .AddScoped<CachingInterceptor>()
            .AddScoped<CachingAsyncInterceptor>();
    }
}