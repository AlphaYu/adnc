using Adnc.Infra.Consul.Consumer;
using Adnc.Infra.EfCore.MySQL;
using Adnc.Infra.Mongo.Configuration;
using Adnc.Infra.Mongo.Extensions;
using Adnc.Shared.Application.Channels;
using Adnc.Shared.RpcServices;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Refit;
using SkyApm.Diagnostics.CAP;
using System.Net.Http;

namespace Adnc.Shared.Application.Registrar;

public abstract class AbstractApplicationDependencyRegistrar : IDependencyRegistrar
{
    public abstract Assembly ApplicationAssembly { get; }
    public abstract Assembly ContractsAssembly { get; }
    public abstract Assembly RepositoryOrDomainAssembly { get; }
    public string Name => "application";
    public virtual string ASPNETCORE_ENVIRONMENT => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    public virtual bool IsDevelopment => ASPNETCORE_ENVIRONMENT.EqualsIgnoreCase("Development");
    protected virtual List<Type> DefaultInterceptorTypes => new() { typeof(OperateLogInterceptor), typeof(CachingInterceptor), typeof(UowInterceptor) };
    protected IServiceCollection Services { get; private set; }
    protected IConfiguration Configuration { get; private set; }
    protected IServiceInfo ServiceInfo { get; private set; }
    protected ProxyGenerator CastleProxyGenerator => new();
    protected IConfigurationSection RedisSection { get; private set; }
    protected IConfigurationSection ConsulSection { get; private set; }
    protected IConfigurationSection RabbitMqSection { get; private set; }
    protected IConfigurationSection MongoDbSection { get; private set; }
    protected IConfigurationSection MysqlSection { get; private set; }

    protected AbstractApplicationDependencyRegistrar(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentException("IServiceCollection is null."); ;
        Configuration = services.GetConfiguration() ?? throw new ArgumentException("Configuration is null.");
        ServiceInfo = services.GetServiceInfo() ?? throw new ArgumentException("ServiceInfo is null.");
        RedisSection = Configuration.GetRedisSection() ?? throw new ArgumentException("RedisSection is null.");
        MongoDbSection = Configuration.GetMongoDbSection() ?? throw new ArgumentException("MongoDbSection is null.");
        MysqlSection = Configuration.GetMysqlSection() ?? throw new ArgumentException("MysqlSection is null.");
        ConsulSection = Configuration.GetConsulSection();
        RabbitMqSection = Configuration.GetRabbitMqSection();
    }

    public abstract void AddAdnc();

    /// <summary>
    /// 注册Shared.Application通用服务
    /// </summary>
    protected void AddApplicationSharedServices()
    {
        Services.AddSingleton(typeof(Lazy<>));
        //https://andrewlock.net/how-to-register-a-service-with-multiple-interfaces-for-in-asp-net-core-di/
        Services.AddScoped<IUserContext,UserContext>();
        Services.AddScoped<OperateLogInterceptor>();
        Services.AddScoped<OperateLogAsyncInterceptor>();
        Services.AddScoped<UowInterceptor>();
        Services.AddScoped<UowAsyncInterceptor>();
        Services.AddSingleton<IBloomFilterFactory, DefaultBloomFilterFactory>();
        Services.AddSingleton<WorkerNode>();
        Services.AddHostedService<CachingHostedService>();
        Services.AddHostedService<WorkerNodeHostedService>();
        Services.AddHostedService<ChannelConsumersHostedService>();
        Services.AddHostedService<BloomFilterHostedService>();
    }

    /// <summary>
    /// 注册EFCoreContext与仓储
    /// </summary>
    protected virtual void AddEfCoreContextWithRepositories(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var serviceType = typeof(IEntityInfo);
        var implType = RepositoryOrDomainAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
        if (implType is null)
            throw new NullReferenceException(nameof(IEntityInfo));
        else
            Services.AddSingleton(serviceType, implType);

        Services.AddScoped(provider =>
        {
            var userContext = provider.GetRequiredService<IUserContext>();
            return new Operater
            {
                Id = userContext.Id,
                Account = userContext.Account,
                Name = userContext.Name
            };
        });

        Services.AddAdncInfraEfCoreMySql();

        var mysqlConfig = MysqlSection.Get<MysqlConfig>();
        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
        Services.AddDbContext<AdncDbContext>(options =>
        {
            options.UseMySql(mysqlConfig.ConnectionString, serverVersion, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .CommandTimeout(10)
                                        .MigrationsAssembly(ServiceInfo.AssemblyName.Replace("WebApi", "Migrations"))
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            if (IsDevelopment)
            {
                //options.AddInterceptors(new DefaultDbCommandInterceptor())
                options.LogTo(Console.WriteLine, LogLevel.Information)
                            //.EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
            }
            //替换默认查询sql生成器,如果通过mycat中间件实现读写分离需要替换默认SQL工厂。
            //options.ReplaceService<IQuerySqlGeneratorFactory, AdncMySqlQuerySqlGeneratorFactory>();
        });
    }

    /// <summary>
    /// 注册MongoContext与仓储
    /// </summary>
    protected virtual void AddMongoContextWithRepositries(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var mongoConfig = MongoDbSection.Get<MongoConfig>();
        Services.AddAdncInfraMongo<MongoContext>(options =>
        {
            options.ConnectionString = mongoConfig.ConnectionString;
            options.PluralizeCollectionNames = mongoConfig.PluralizeCollectionNames;
            options.CollectionNamingConvention = (NamingConvention)mongoConfig.CollectionNamingConvention;
        });
    }

    /// <summary>
    /// 注册CAP组件的订阅者(实现事件总线及最终一致性（分布式事务）的一个开源的组件)
    /// </summary>
    /// <param name="tableNamePrefix">cap表面前缀</param>
    /// <param name="groupName">群组名子</param>
    /// <param name="func">回调函数</param>
    protected virtual void AddEventBusSubscribers<TSubscriber>(Action<IServiceCollection> action = null)
        where TSubscriber : class, ICapSubscribe
    {
        action?.Invoke(Services);

        var tableNamePrefix = "cap";
        var groupName = $"cap.{ServiceInfo.ShortName}.{ASPNETCORE_ENVIRONMENT[..3]}".ToLower();

        //add skyamp
        //Services.AddSkyApmExtensions().AddCap();

        Services.AddSingleton<TSubscriber>();

        var rabbitMqConfig = Configuration.GetRabbitMqSection().Get<RabbitMqConfig>();
        Services.AddCap(x =>
        {
            var mysqlConfig = Configuration.GetMysqlSection().Get<MysqlConfig>();
            x.UseMySql(config =>
            {
                config.ConnectionString = mysqlConfig.ConnectionString;
                config.TableNamePrefix = tableNamePrefix;
            });

            //CAP支持 RabbitMQ、Kafka、AzureServiceBus 等作为MQ，根据使用选择配置：
            x.UseRabbitMQ(option =>
            {
                option.HostName = rabbitMqConfig.HostName;
                option.VirtualHost = rabbitMqConfig.VirtualHost;
                option.Port = rabbitMqConfig.Port;
                option.UserName = rabbitMqConfig.UserName;
                option.Password = rabbitMqConfig.Password;
            });
            x.Version = ServiceInfo.Version;
            //默认值：cap.queue.{程序集名称},在 RabbitMQ 中映射到 Queue Names。
            x.DefaultGroupName = groupName;
            //默认值：60 秒,重试 & 间隔
            //在默认情况下，重试将在发送和消费消息失败的 4分钟后 开始，这是为了避免设置消息状态延迟导致可能出现的问题。
            //发送和消费消息的过程中失败会立即重试 3 次，在 3 次以后将进入重试轮询，此时 FailedRetryInterval 配置才会生效。
            x.FailedRetryInterval = 60;
            //默认值：50,重试的最大次数。当达到此设置值时，将不会再继续重试，通过改变此参数来设置重试的最大次数。
            x.FailedRetryCount = 50;
            //默认值：NULL,重试阈值的失败回调。当重试达到 FailedRetryCount 设置的值的时候，将调用此 Action 回调
            //，你可以通过指定此回调来接收失败达到最大的通知，以做出人工介入。例如发送邮件或者短信。
            x.FailedThresholdCallback = (failed) =>
            {
                //todo
            };
            //默认值：24*3600 秒（1天后),成功消息的过期时间（秒）。
            //当消息发送或者消费成功时候，在时间达到 SucceedMessageExpiredAfter 秒时候将会从 Persistent 中删除，你可以通过指定此值来设置过期的时间。
            x.SucceedMessageExpiredAfter = 24 * 3600;
            //默认值：1,消费者线程并行处理消息的线程数，当这个值大于1时，将不能保证消息执行的顺序。
            x.ConsumerThreadCount = 1;
            x.UseDashboard(x =>
            {
                x.PathMatch = $"/{ServiceInfo.ShortName}/cap";
                x.UseAuth = false;
            });

            /* CAP目前不需要自动注册，先注释
            //必须是生产环境才注册cap服务到consul
            if ((_environment.IsProduction() || _environment.IsStaging()))
            {
                x.UseDiscovery(discoverOptions =>
                {
                    var consulConfig = _configuration.GetConsulSection().Get<ConsulConfig>();
                    var consulAdderss = new Uri(consulConfig.ConsulUrl);

                    var hostIps = NetworkInterface
                                                                    .GetAllNetworkInterfaces()
                                                                    .Where(network => network.OperationalStatus == OperationalStatus.Up)
                                                                    .Select(network => network.GetIPProperties())
                                                                    .OrderByDescending(properties => properties.GatewayAddresses.Count)
                                                                    .SelectMany(properties => properties.UnicastAddresses)
                                                                    .Where(address => !IPAddress.IsLoopback(address.Address) && address.Address.AddressFamily == AddressFamily.InterNetwork)
                                                                    .ToArray();

                    var currenServerAddress = hostIps.First().Address.MapToIPv4().ToString();

                    discoverOptions.DiscoveryServerHostName = consulAdderss.Host;
                    discoverOptions.DiscoveryServerPort = consulAdderss.Port;
                    discoverOptions.CurrentNodeHostName = currenServerAddress;
                    discoverOptions.CurrentNodePort = 80;
                    discoverOptions.NodeId = DateTime.Now.Ticks.ToString();
                    discoverOptions.NodeName = _serviceInfo.FullName.Replace("webapi", "cap");
                    discoverOptions.MatchPath = $"/{_serviceInfo.ShortName}/cap";
                });
            }
            */
        });
    }

    /// <summary>
    /// 注册Rpc服务(跨微服务之间的同步通讯)
    /// </summary>
    /// <typeparam name="TRpcService">Rpc服务接口</typeparam>
    /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
    /// <param name="policies">Polly策略</param>
    protected virtual void AddRpcService<TRpcService>(string serviceName, List<IAsyncPolicy<HttpResponseMessage>> policies)
     where TRpcService : class, IRpcService
    {
        var prefix = serviceName[..7];
        bool isConsulAdderss = prefix != "http://" && prefix != "https:/";
        //如果参数是服务名字，那么需要从consul获取地址
        var baseAddress = isConsulAdderss ? $"http://{serviceName}" : serviceName;

        //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
        var refitSettings = new RefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(SystemTextJson.GetAdncDefaultOptions())
        };
        var clientbuilder = Services.AddRefitClient<TRpcService>(refitSettings)
                                                     .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                                                     .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(baseAddress));
        if (isConsulAdderss)
            clientbuilder.AddHttpMessageHandler<ConsulDiscoverDelegatingHandler>();
        else
            clientbuilder.AddHttpMessageHandler<SimpleDiscoveryDelegatingHandler>();

        //添加polly相关策略
        policies?.ForEach(policy => clientbuilder.AddPolicyHandler(policy));
    }

    /// <summary>
    /// 注册Application服务
    /// </summary>
    protected virtual void AddAppliactionSerivcesWithInterceptors(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var appServiceType = typeof(IAppService);
        var serviceTypes = ContractsAssembly.GetExportedTypes().Where(type => type.IsInterface && type.IsAssignableTo(appServiceType)).ToList();
        serviceTypes.Remove(appServiceType);
        var lifetime = ServiceLifetime.Scoped;
        if (serviceTypes.IsNullOrEmpty())
            return;
        serviceTypes.ForEach(serviceType =>
        {
            var implType = ApplicationAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implType is null)
                return;

            Services.Add(new ServiceDescriptor(implType, implType, lifetime));

            var serviceDescriptor = new ServiceDescriptor(serviceType, provider =>
            {
                var interceptors = DefaultInterceptorTypes.ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor).ToArray();
                var target = provider.GetService(implType);
                var interfaceToProxy = serviceType;
                var proxy = CastleProxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, target, interceptors);
                return proxy;
            }, lifetime);
            Services.Add(serviceDescriptor);
        });
    }

    /// <summary>
    /// 注册Domain服务
    /// </summary>
    protected virtual void AddDomainSerivces<TDomainService>(Action<IServiceCollection> action = null)
        where TDomainService : class
    {
        action?.Invoke(Services);

        var serviceType = typeof(TDomainService);
        var implTypes = RepositoryOrDomainAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implTypes.ForEach(implType =>
        {
            Services.AddScoped(implType, implType);
        });
    }

    /// <summary>
    /// 注册Application的IHostedService服务
    /// </summary>
    protected virtual void AddApplicaitonHostedServices()
    {
        var serviceType = typeof(IHostedService);
        var implTypes = ApplicationAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implTypes.ForEach(implType =>
        {
            Services.AddSingleton(serviceType, implType);
        });
    }

    /// <summary>
    /// 注册Caching相关处理服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void AddCachingServices(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        Services.AddAdncInfraCaching(RedisSection);
        var serviceType = typeof(ICachePreheatable);
        var implTypes = ApplicationAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        if (implTypes.IsNotNullOrEmpty())
        {
            implTypes.ForEach(implType =>
            {
                Services.AddSingleton(implType, implType);
                Services.AddSingleton(x => x.GetRequiredService(implType) as ICachePreheatable);
            });
        }
    }

    /// <summary>
    /// 注册BloomFilter相关处理服务
    /// </summary>
    /// <param name="builder"></param>
    protected virtual void AddBloomFilterServices(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var serviceType = typeof(IBloomFilter);
        var implTypes = ApplicationAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        if (implTypes.IsNotNullOrEmpty())
            implTypes.ForEach(implType => Services.AddSingleton(serviceType, implType));
    }

    /// <summary>
    /// 注册Consul相关服务
    /// </summary>
    /// <param name="action"></param>
    protected virtual void AddConsulServices(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        if (ConsulSection is not null)
            Services.AddAdncInfraConsul(ConsulSection);
    }

    /// <summary>
    /// 注册事件发布者相关服务
    /// </summary>
    /// <param name="action"></param>
    protected virtual void AddEventBusPublishers(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        if (RabbitMqSection is not null)
            Services.AddAdncInfraEventBus();
    }
}