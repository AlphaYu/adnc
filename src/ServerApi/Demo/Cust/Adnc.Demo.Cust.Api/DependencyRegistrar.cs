using Adnc.Demo.Cust.Api.Application.Subscribers;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Cust.Api;

public sealed class ApiLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : AbstractWebApiDependencyRegistrar(services, serviceInfo)
{
    public override void AddAdncServices()
    {
        Services.AddSingleton(typeof(IServiceInfo), ServiceInfo);

        var registrar = new ApplicationLayerRegistrar(Services, ServiceInfo);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();
        AddHealthChecks(true, true, true, true);
    }
}

public sealed class ApplicationLayerRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : AbstractApplicationDependencyRegistrar(services,serviceInfo)
{
    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    public override Assembly ApplicationLayerAssembly => _assembly;
    public override Assembly ContractsLayerAssembly => _assembly;
    public override Assembly RepositoryOrDomainLayerAssembly => _assembly;

    public override void AddApplicationServices()
    {
        //register base services
        AddApplicaitonDefault();
        //register rpc-http services
        var restPolicies = PollyStrategyEnable ? this.GenerateDefaultRefitPolicies() : new();
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.AdncDemoAuthService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.AdncDemoUsrService, restPolicies);
        AddRestClient<IMaintRestClient>(ServiceAddressConsts.AdncDemoMaintService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.AdncDemoWhseService, restPolicies);

        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AuthGrpc.AuthGrpcClient>(ServiceAddressConsts.AdncDemoAuthService, gprcPolicies);
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(ServiceAddressConsts.AdncDemoUsrService, gprcPolicies);
        AddGrpcClient<MaintGrpc.MaintGrpcClient>(ServiceAddressConsts.AdncDemoMaintService, gprcPolicies);
        AddGrpcClient<WhseGrpc.WhseGrpcClient>(ServiceAddressConsts.AdncDemoWhseService, gprcPolicies);
        //register rpc-event service
        AddCapEventBus<CapEventSubscriber>();
        //register others services
        //Services.AddScoped<xxxx>
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdnc(this IServiceCollection services, IServiceInfo serviceInfo)
    {
        var registrar = new ApiLayerRegistrar(services, serviceInfo);
        registrar.AddAdncServices();
        return services;
    }
}