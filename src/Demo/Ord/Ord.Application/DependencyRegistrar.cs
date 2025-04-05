using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;
using Microsoft.Extensions.Configuration;

namespace Adnc.Demo.Ord.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IOrderService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefaultServices();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.WhseDemoService, restPolicies);
        //rpc-event
        AddCapEventBus([typeof(CapEventSubscriber)]);
    }
}
