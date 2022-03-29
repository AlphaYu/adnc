using Adnc.Infra.Consul;

namespace Adnc.Shared.Application;

/// <summary>
/// Autofac注册
/// </summary>
public abstract class AdncApplicationModule : Autofac.Module
{
    private readonly Assembly _appAssemblieToScan;
    private readonly Assembly _appContractsAssemblieToScan;
    private readonly Assembly _repoAssemblieToScan;
    private readonly Assembly _domainAssemblieToScan;
    private readonly IConfigurationSection _redisSection;
    private readonly IConfigurationSection _consulSection;
    private readonly IServiceInfo _serviceInfo;

    protected AdncApplicationModule(Type modelType, IConfiguration configuration, IServiceInfo serviceInfo, bool isDddDevelopment = false)
    {
        _serviceInfo = serviceInfo;
        _redisSection = configuration.GetRedisSection();
        _consulSection = configuration.GetConsulSection();
        _appAssemblieToScan = modelType?.Assembly ?? throw new ArgumentNullException(nameof(modelType));
        _appContractsAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Application.Contracts"));
        if (isDddDevelopment)
            _domainAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Domain"));
        else
            _repoAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Repository"));
    }

    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {
        RegisterApplicationServices(builder);
        RegisterCachingServices(builder);
        RegisterBloomfilterServices(builder);
        RegisterIdChannelConsumerServices(builder);
        RegisterIdGeneraterServices(builder);
        LoadDepends(builder);
    }

    /// <summary>
    ///注册Appliaction相关服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RegisterApplicationServices(ContainerBuilder builder)
    {
        #region register usercontext,operater

        //注册UserContext(IUserContext,IOperater)
        builder.RegisterType<UserContext>()
                    .As<IUserContext, IOperater>()
                    .InstancePerLifetimeScope();

        #endregion register usercontext,operater

        #region register opslog interceptor

        //注册操作日志拦截器
        builder.RegisterType<OperateLogInterceptor>()
                    .InstancePerLifetimeScope();
        builder.RegisterType<OperateLogAsyncInterceptor>()
                    .InstancePerLifetimeScope();

        #endregion register opslog interceptor

        #region register uow interceptor

        //注册操作日志拦截器
        builder.RegisterType<UowInterceptor>()
                    .InstancePerLifetimeScope();
        builder.RegisterType<UowAsyncInterceptor>()
                    .InstancePerLifetimeScope();

        #endregion register uow interceptor

        #region register appservices,interceptors

        //注册应用服务与拦截器
        var interceptors = new List<Type>
        {
            typeof(OperateLogInterceptor)
            , typeof(CachingInterceptor)
            ,typeof(UowInterceptor)
        };

        builder.RegisterAssemblyTypes(_appAssemblieToScan)
                   .Where(t => t.IsAssignableTo<IAppService>() && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                   .InstancePerLifetimeScope()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(interceptors.ToArray());

        #endregion register appservices,interceptors

        #region register dto validators

        //注册DtoValidators
        builder.RegisterAssemblyTypes(_appContractsAssemblieToScan)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

        #endregion register dto validators
    }

    /// <summary>
    /// 注册Caching相关处理服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RegisterCachingServices(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(_appAssemblieToScan)
           .Where(t => t.IsAssignableTo<ICacheService>() && !t.IsAbstract)
           .AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope()
           .As<ICacheService>().SingleInstance();
        builder.RegisterType<CachingHostedService>()
                    .As<IHostedService>()
                    .InstancePerLifetimeScope();
    }

    /// <summary>
    /// 注册Bloomfilters相关服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RegisterBloomfilterServices(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(_appAssemblieToScan)
           .Where(t => t.IsAssignableTo<IBloomFilter>() && !t.IsAbstract)
           .AsImplementedInterfaces()
           .SingleInstance();
        builder.RegisterType<DefaultBloomFilterFactory>()
                    .As<IBloomFilterFactory>()
                    .SingleInstance();
        builder.RegisterType<BloomFilterHostedService>()
                    .As<IHostedService>()
                    .SingleInstance();
    }

    /// <summary>
    ///注册ChannelConsumers相关服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RegisterIdChannelConsumerServices(ContainerBuilder builder)
    {
        builder.RegisterType<ChannelConsumersHostedService>()
                    .As<IHostedService>()
                    .SingleInstance();
    }

    /// <summary>
    ///   注册Id生成器相关服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void RegisterIdGeneraterServices(ContainerBuilder builder)
    {
        builder.RegisterType<WorkerNode>()
                    .AsSelf()
                    .SingleInstance();
        builder.RegisterType<WorkerNodeHostedService>()
                    .As<IHostedService>()
                    .WithParameter("serviceName", _serviceInfo.ShortName)
                    .SingleInstance();
    }

    /// <summary>
    /// 注册依赖模块相关服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void LoadDepends(ContainerBuilder builder)
    {
        builder.RegisterModuleIfNotRegistered(new AdncInfraEventBusModule(_appAssemblieToScan));
        builder.RegisterModuleIfNotRegistered(new AdncInfraAutoMapperModule(_appAssemblieToScan));
        builder.RegisterModuleIfNotRegistered(new AdncInfraCachingModule(_redisSection));
        builder.RegisterModuleIfNotRegistered<AdncInfraMongoModule>();
        builder.RegisterModuleIfNotRegistered<AdncInfraEfCoreModule>();
        builder.RegisterModuleIfNotRegistered(new AdncInfraConsulModule(_consulSection.Get<ConsulConfig>().ConsulUrl));
        //builder.RegisterModuleIfNotRegistered(new AdncInfraHangfireModule(_appAssemblieToScan));

        if (_domainAssemblieToScan != null)
        {
            var modelType = _domainAssemblieToScan.GetTypes().FirstOrDefault(x => x.IsAssignableTo<Autofac.Module>() && !x.IsAbstract);
            var adncDomainModule = System.Activator.CreateInstance(modelType) as Autofac.Module;
            builder.RegisterModuleIfNotRegistered(adncDomainModule);
        }
        if (_repoAssemblieToScan != null)
        {
            var modelType = _repoAssemblieToScan.GetTypes().FirstOrDefault(x => x.IsAssignableTo<Autofac.Module>() && !x.IsAbstract);
            var adncRepositoryModule = System.Activator.CreateInstance(modelType) as Autofac.Module;
            builder.RegisterModuleIfNotRegistered(adncRepositoryModule);
        }
    }
}