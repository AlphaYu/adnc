using Adnc.Infra.Repository.Interceptor.Castle;
using Adnc.Shared.Application.Mapper.AutoMapper;
using Adnc.Shared.Application.Services.Trackers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adnc.Shared.Application.Registrar;

/// <summary>
///  Application依赖注册器
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
    public abstract Assembly ApplicationLayerAssembly { get; }
    public abstract Assembly ContractsLayerAssembly { get; }
    public abstract Assembly RepositoryOrDomainLayerAssembly { get; }
    protected List<Type> DefaultInterceptorTypes => [typeof(OperateLogInterceptor), typeof(CachingInterceptor), typeof(UowInterceptor)];
    protected IServiceCollection Services { get; init; }
    protected IConfiguration Configuration { get; init; }
    protected IServiceInfo ServiceInfo { get; init; }
    protected ServiceLifetime Lifetime { get; init; }

    /// <summary>
    /// 注册所有服务
    /// </summary>
    public abstract void AddApplicationServices();

    /// <summary>
    /// 注册adnc.application通用服务
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
            .AddAutoMapper([ApplicationLayerAssembly])
            .AddValidatorsFromAssembly(ContractsLayerAssembly, Lifetime)
            .AddAdncInfraYitterIdGenerater(redisSection, ServiceInfo.ShortName.Split('-')[0], Lifetime)
            .AddAdncInfraConsul(consulSection, null, Lifetime)
            .AddAdncInfraRedisCaching(ApplicationLayerAssembly, redisSection, cachingSection, Lifetime)
            .AddAdncInfraDapper(Lifetime);

        AddEfCoreContext();

        var serviceTypes = ContractsLayerAssembly.GetExportedTypes().Where(type => type.IsInterface && type.IsAssignableTo(typeof(IAppService))).ToList();
        serviceTypes?.ForEach(serviceType =>
        {
            var implType = ApplicationLayerAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implType is not null)
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
