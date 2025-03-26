using Adnc.Demo.Remote.Grpc.Services;
using Adnc.Demo.Remote.Http.Services;

namespace Adnc.Demo.Cust.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdnc(this IServiceCollection services, IServiceInfo serviceInfo)
    {
        var registrar = new ApiLayerRegistrar(services, serviceInfo);
        registrar.AddAdncServices();
        return services;
    }
}

public sealed class ApiLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : AbstractWebApiDependencyRegistrar(services, serviceInfo)
{
    public override void AddAdncServices()
    {
        Services.AddSingleton(typeof(IServiceInfo), ServiceInfo);

        var registrar = new ApplicationLayerRegistrar(Services, ServiceInfo);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        Services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddMySql(Configuration)
                    .AddRedis(Configuration)
                    .AddRabbitMQ(Configuration, ServiceInfo.Id);
        });

        Services.AddCapDashboardStandalone(options =>
        {
            options.PathMatch = $"/{ServiceInfo.RelativeRootPath}/cap";
            options.AllowAnonymousExplicit = false;
            options.AuthorizationPolicy = AuthorizePolicy.Default;
        });

        //register others services
        //Services.AddScoped<xxxx>
    }
}

public sealed class ApplicationLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : AbstractApplicationDependencyRegistrar(services, serviceInfo)
{
    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public override Assembly ApplicationLayerAssembly => _assembly;
    public override Assembly ContractsLayerAssembly => _assembly;
    public override Assembly RepositoryOrDomainLayerAssembly => _assembly;

    public override void AddApplicationServices()
    {
        //register default services
        AddApplicaitonDefault();
        //register http services
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
        //register grpc services
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AdminGrpc.AdminGrpcClient>(ServiceAddressConsts.AdminDemoService, gprcPolicies);
        //register event service
        AddCapEventBus<CapEventSubscriber>();
    }
}