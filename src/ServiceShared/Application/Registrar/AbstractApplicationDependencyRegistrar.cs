using Adnc.Infra.EventBus.Tracker;
using Adnc.Infra.Repository.Interceptor.Castle;
using Adnc.Shared.Application.Mapper.AutoMapper;
using Adnc.Shared.Application.Services.Trackers;
using Castle.DynamicProxy.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adnc.Shared.Application.Registrar;

/// <summary>
/// Application dependency registrar
/// </summary>
public abstract partial class AbstractApplicationDependencyRegistrar
{
    public AbstractApplicationDependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ArgumentNullException.ThrowIfNull(services, $"{nameof(IServiceCollection)} is null.");
        ArgumentNullException.ThrowIfNull(serviceInfo, $"{nameof(IServiceInfo)} is null.");
        ArgumentNullException.ThrowIfNull(serviceLifetime, $"{nameof(ServiceLifetime)} is null.");
        ArgumentNullException.ThrowIfNull(configuration, $"{nameof(IConfiguration)} is null.");

        Services = services;
        ServiceInfo = serviceInfo;
        Lifetime = serviceLifetime;
        Configuration = configuration;
    }

    public string Name => "application";
    protected abstract Assembly ApplicationLayerAssembly { get; }
    //public abstract Assembly ContractsLayerAssembly { get; }
    protected abstract Assembly RepositoryOrDomainLayerAssembly { get; }
    protected List<Type> DefaultInterceptorTypes => [typeof(OperateLogInterceptor), typeof(CachingInterceptor), typeof(UowInterceptor)];
    internal IServiceCollection Services { get; init; }
    internal IConfiguration Configuration { get; init; }
    internal IServiceInfo ServiceInfo { get; init; }
    internal ServiceLifetime Lifetime { get; init; }

    /// <summary>
    /// Registers all services.
    /// </summary>
    public abstract void AddApplicationServices();

    /// <summary>
    /// Registers common adnc.application services.
    /// </summary>
    protected void AddApplicaitonDefaultServices()
    {
        Services.TryAddSingleton(ServiceInfo);

        Services.TryAddSingleton(typeof(Lazy<>));
        Services.Add(new ServiceDescriptor(typeof(UserContext), typeof(UserContext), Lifetime));

        Services.Add(new ServiceDescriptor(typeof(IMessageTracker), typeof(DbMessageTrackerService), Lifetime));
        Services.Add(new ServiceDescriptor(typeof(IMessageTracker), typeof(RedisMessageTrackerService), Lifetime));
        Services.Add(new ServiceDescriptor(typeof(MessageTrackerFactory), typeof(MessageTrackerFactory), Lifetime));

        Services.AddHostedService<Channels.LogConsumersHostedService>();

        Services.Add(new ServiceDescriptor(typeof(OperateLogInterceptor), typeof(OperateLogInterceptor), Lifetime));
        Services.Add(new ServiceDescriptor(typeof(OperateLogAsyncInterceptor), typeof(OperateLogAsyncInterceptor), Lifetime));

        var redisSection = Configuration.GetRequiredSection(NodeConsts.Redis);
        var cachingSection = Configuration.GetRequiredSection(NodeConsts.Caching);
        var consulSection = Configuration.GetRequiredSection(NodeConsts.Consul);
        Services
            .AddSingleton<IObjectMapper, AutoMapperObject>()
            .AddAutoMapper(cfg => { }, ApplicationLayerAssembly)
            .AddValidatorsFromAssembly(ApplicationLayerAssembly, Lifetime)
            .AddAdncInfraYitterIdGenerater(redisSection, ServiceInfo.ShortName.Split('-')[0], Lifetime)
            .AddAdncInfraConsul(consulSection, null, Lifetime)
            .AddAdncInfraRedisCaching(ApplicationLayerAssembly, redisSection, cachingSection, Lifetime)
            .AddAdncInfraDapper(Lifetime);

        AddEfCoreContext();

        var implTypes = ApplicationLayerAssembly.GetExportedTypes().Where(type => type.IsClass && type.IsNotAbstractClass(true) && type.IsAssignableTo(typeof(IAppService))).ToList();
        implTypes?.ForEach(implType =>
        {
            var allInterfaces = implType.GetAllInterfaces().ToList();
            var serviceType = allInterfaces.FirstOrDefault(x => x.IsAssignableTo(typeof(IAppService)));
            if (serviceType is not null)
            {
                Services.TryAddSingleton(new ProxyGenerator());
                Services.Add(new ServiceDescriptor(implType, implType, Lifetime));
                Services.Add(new ServiceDescriptor(serviceType, provider =>
                {
                    var interfaceToProxy = serviceType;
                    var target = provider.GetRequiredService(implType);
                    var interceptors = DefaultInterceptorTypes.ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor).ToArray();
                    var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                    var proxy = proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, target, interceptors);
                    return proxy;
                }, Lifetime));
            }
        });
    }
}
