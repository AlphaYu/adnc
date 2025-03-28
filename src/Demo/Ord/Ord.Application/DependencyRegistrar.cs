using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;

namespace Adnc.Demo.Ord.Application;

public sealed class DependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IOrderService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : base(services, serviceInfo)
    {
    }

    public override void AddApplicationServices()
    {
        AddApplicaitonDefault();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.WhseDemoService, restPolicies);
        //rpc-event
        AddCapEventBus<CapEventSubscriber>();
    }
}