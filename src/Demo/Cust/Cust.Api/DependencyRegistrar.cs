using Adnc.Demo.Remote.Grpc.Services;
using Adnc.Demo.Remote.Http.Services;

namespace Adnc.Demo.Cust.Api;

public sealed class ApiLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration) : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    public override void AddAdncServices()
    {
        var registrar = new ApplicationLayerRegistrar(Services, ServiceInfo, Configuration);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        var mysqlSection = Configuration.GetRequiredSection(NodeConsts.Mysql);
        var redisSecton = Configuration.GetRequiredSection(NodeConsts.Redis);
        var rabbitSecton = Configuration.GetRequiredSection(NodeConsts.RabbitMq);
        var clientProvidedName = ServiceInfo.Id;
        Services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddMySql(mysqlSection)
                    .AddRedis(redisSecton)
                    .AddRabbitMQ(rabbitSecton, clientProvidedName);
        });

        var capPath = $"/{ServiceInfo.RelativeRootPath}/cap";
        var capPolicy = AuthorizePolicy.Default;
        Services.AddCapDashboardStandalone(options =>
        {
            options.PathMatch = capPath;
            options.AllowAnonymousExplicit = false;
            options.AuthorizationPolicy = capPolicy;
        });

        //register others services
        //Services.AddScoped<xxxx>
    }
}

public sealed class ApplicationLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
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
