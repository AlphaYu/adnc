using Adnc.Shared.Application.Caching.SkyApm;
using Adnc.Shared.Application.Extensions;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册Caching相关处理服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void AddRedisCaching(Action<IServiceCollection>? action = null)
    {
        action?.Invoke(Services);
        if (this.IsEnableSkyApm())
        {
            SkyApm.AddRedisCaching();
        }
        Services.AddAdncInfraRedisCaching(RedisSection, CachingSection);

        var serviceType = typeof(ICachePreheatable);
        var implType = ApplicationLayerAssembly.ExportedTypes.SingleOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
        if (implType is not null)
        {
            Services.AddSingleton(implType);
            Services.AddSingleton(x => (ICachePreheatable)x.GetRequiredService(implType));
            Services.AddHostedService<CachingHostedService>();
        }
    }

    /// <summary>
    /// 注册BloomFilter相关处理服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void AddBloomFilters(Action<IServiceCollection>? action = null)
    {
        action?.Invoke(Services);

        var serviceType = typeof(IBloomFilter);
        var implTypes = ApplicationLayerAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        if (implTypes.IsNotNullOrEmpty())
        {
            Services.AddSingleton<IBloomFilter, NullBloomFilter>();
            Services.AddSingleton<BloomFilterFactory>();
            Services.AddHostedService<BloomFilterHostedService>();

            implTypes.ForEach(implType => Services.AddSingleton(serviceType, implType));
        }
    }
}