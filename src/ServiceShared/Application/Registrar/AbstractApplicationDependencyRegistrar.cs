﻿using Adnc.Shared.Application.Mapper.AutoMapper;
using Adnc.Shared.Application.Services.Trackers;
using Adnc.Shared.Remote;

namespace Adnc.Shared.Application.Registrar;

//public abstract partial class AbstractApplicationDependencyRegistrar : IDependencyRegistrar
public abstract partial class AbstractApplicationDependencyRegistrar
{
    public string Name => "application";
    public abstract Assembly ApplicationLayerAssembly { get; }
    public abstract Assembly ContractsLayerAssembly { get; }
    public abstract Assembly RepositoryOrDomainLayerAssembly { get; }
    protected SkyApmExtensions SkyApm { get; init; }
    protected string RegisterType { get; init; }
    protected RpcInfo? RpcInfoOption { get; init; }
    protected IServiceCollection Services { get; init; }
    protected IConfiguration Configuration { get; init; }
    protected IServiceInfo ServiceInfo { get; init; }
    protected IConfigurationSection RedisSection { get; init; }
    protected IConfigurationSection CachingSection { get; init; }
    protected IConfigurationSection MysqlSection { get; init; }
    protected IConfigurationSection MongoDbSection { get; init; }
    protected IConfigurationSection ConsulSection { get; init; }
    protected IConfigurationSection RabbitMqSection { get; init; }
    protected virtual Microsoft.EntityFrameworkCore.ServerVersion DbVersion { get; set; } = new Microsoft.EntityFrameworkCore.MariaDbServerVersion(new Version(10, 5, 4));

    public AbstractApplicationDependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services), $"{nameof(IServiceCollection)} is null.");
        ServiceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo), $"{nameof(IServiceInfo)} is null.");
        Configuration = services.GetConfiguration() ?? throw new InvalidDataException("Configuration is null.");
        RedisSection = Configuration.GetRequiredSection(NodeConsts.Redis);
        CachingSection = Configuration.GetRequiredSection(NodeConsts.Caching);
        MongoDbSection = Configuration.GetSection(NodeConsts.MongoDb);
        MysqlSection = Configuration.GetSection(NodeConsts.Mysql);
        ConsulSection = Configuration.GetSection(NodeConsts.Consul);
        RabbitMqSection = Configuration.GetSection(NodeConsts.RabbitMq);
        SkyApm = Services.AddSkyApmExtensions();
        RegisterType = Configuration.GetValue<string>(NodeConsts.RegisterType) ?? RegisteredTypeConsts.Direct;
        RpcInfoOption = Configuration.GetSection(NodeConsts.RpcInfo).Get<RpcInfo>();
    }

    /// <summary>
    /// 注册所有服务
    /// </summary>
    public abstract void AddApplicationServices();

    /// <summary>
    /// 注册adnc.application通用服务
    /// </summary>
    protected virtual void AddApplicaitonDefault()
    {
        Services
            .AddSingleton(typeof(Lazy<>))
            .AddScoped<UserContext>()
            .AddScoped<IMessageTracker, DbMessageTrackerService>()
            .AddScoped<IMessageTracker, RedisMessageTrackerService>()
            .AddScoped<MessageTrackerFactory>()
            .AddHostedService<Channels.LogConsumersHostedService>()
            .AddAutoMapper(ApplicationLayerAssembly)
            .AddSingleton<IObjectMapper, AutoMapperObject>()
            .AddValidatorsFromAssembly(ContractsLayerAssembly, ServiceLifetime.Scoped)
            .AddAdncInfraYitterIdGenerater(RedisSection, ServiceInfo.ShortName.Split('-')[0])
            .AddAdncInfraConsul(ConsulSection)
            .AddAdncInfraDapper();

        AddAppliactionSerivcesWithInterceptors();
        AddEfCoreContextWithRepositories();
        AddRedisCaching();
        AddBloomFilters();
    }
}
