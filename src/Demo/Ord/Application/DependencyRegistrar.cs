using Adnc.Demo.Ord.Application.EventSubscribers;
using Adnc.Demo.Remote.Http;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;
using Microsoft.Extensions.Configuration;

namespace Adnc.Demo.Ord.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    //private readonly IServiceCollection _services = services;
    //private readonly IServiceInfo _serviceInfo = serviceInfo;
    //private readonly IConfiguration _configuration = configuration;

    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefaultServices();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.WhseDemoService, restPolicies);
        //rpc-event
        AddCapEventBus([typeof(WarehouseQtyBlockedEventSubscriber)]);
    }
}
