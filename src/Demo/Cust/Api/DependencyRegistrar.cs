using Adnc.Demo.Remote.Grpc.Services;
using Adnc.Demo.Remote.Http.Services;

namespace Adnc.Demo.Cust.Api;

public sealed class ApiLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration) : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    private readonly IServiceCollection _services = services;
    private readonly IServiceInfo _serviceInfo = serviceInfo;
    private readonly IConfiguration _configuration = configuration;

    public override void AddAdncServices()
    {
        var registrar = new ApplicationLayerRegistrar(_services, _serviceInfo, _configuration);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        var mysqlSection = _configuration.GetRequiredSection(NodeConsts.Mysql);
        var redisSecton = _configuration.GetRequiredSection(NodeConsts.Redis);
        var rabbitSecton = _configuration.GetRequiredSection(NodeConsts.RabbitMq);
        var clientProvidedName = _serviceInfo.Id;
        _services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddMySql(mysqlSection)
                    .AddRedis(redisSecton)
                    .AddRabbitMQ(rabbitSecton, clientProvidedName);
        });

        var capPath = $"/{_serviceInfo.RelativeRootPath}/cap";
        var capPolicy = AuthorizePolicy.Default;
        _services.AddCapDashboardStandalone(options =>
        {
            options.PathMatch = capPath;
            options.AllowAnonymousExplicit = false;
            options.AuthorizationPolicy = capPolicy;
        });

        //register others services
        //_services.AddScoped<xxxx>
    }
}

public sealed class ApplicationLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    //private readonly IServiceCollection _services = services;
    //private readonly IServiceInfo _serviceInfo = serviceInfo;
    //private readonly IConfiguration _configuration = configuration;
    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public override Assembly ApplicationLayerAssembly => _assembly;
    public override Assembly ContractsLayerAssembly => _assembly;
    public override Assembly RepositoryOrDomainLayerAssembly => _assembly;

    public override void AddApplicationServices()
    {
        //register default services
        AddApplicaitonDefaultServices();
        //register http services
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
        //register grpc services
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AdminGrpc.AdminGrpcClient>(ServiceAddressConsts.AdminDemoService, gprcPolicies);
        //register event service
        AddCapEventBus([typeof(CapEventSubscriber)]);
    }
}
