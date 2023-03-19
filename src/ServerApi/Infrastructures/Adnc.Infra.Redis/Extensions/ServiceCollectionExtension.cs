using Adnc.Infra.Redis;
using Adnc.Infra.Redis.Configurations;
using Adnc.Infra.Redis.Core;
using Adnc.Infra.Redis.Core.Serialization;
using StackExchangeProvider = Adnc.Infra.Redis.Providers.StackExchange;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraRedis(this IServiceCollection services, IConfigurationSection redisSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraRedis)))
            return services;

        var redisConfig = redisSection.Get<RedisOptions>();
        services.Configure<RedisOptions>(redisSection);
        services.AddSingleton(provider =>
        {
            var serializerType = typeof(ISerializer);
            var scanedAssembly = typeof(ISerializer).Assembly;
            var serializers = scanedAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serializerType) && type.IsNotAbstractClass(true));
            var serializerName = string.IsNullOrWhiteSpace(redisConfig.SerializerName) ? ConstValue.Serializer.DefaultBinarySerializerName : redisConfig.SerializerName;
            var instanceType = serializers.Single(x => x.Name.Contains(serializerName, StringComparison.CurrentCultureIgnoreCase));
            return (ISerializer)ActivatorUtilities.CreateInstance(provider, instanceType);
        });

        switch (redisConfig.Provider)
        {
            case ConstValue.Provider.StackExchange:
                AddAdncStackExchange(services);
                break;

            case ConstValue.Provider.ServiceStack:
                break;

            case ConstValue.Provider.FreeRedis:
                break;

            case ConstValue.Provider.CSRedis:
                break;

            default:
                throw new NotSupportedException(nameof(redisConfig.Provider));
        }

        return services;
    }

    public static IServiceCollection AddAdncStackExchange(IServiceCollection services)
    {
        return
            services
            .AddSingleton<StackExchangeProvider.DefaultDatabaseProvider>()
            .AddSingleton<StackExchangeProvider.DefaultRedisProvider>()
            .AddSingleton<IRedisProvider>(x => x.GetRequiredService<StackExchangeProvider.DefaultRedisProvider>())
            .AddSingleton<IDistributedLocker>(x => x.GetRequiredService<StackExchangeProvider.DefaultRedisProvider>());
    }
}