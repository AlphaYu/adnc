using Adnc.Shared.Application.Registrar;

namespace Adnc.Ord.Application;

public sealed class OrdApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IOrderAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public OrdApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddApplicaitonDefault();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.UsrService, restPolicies);
        AddRestClient<IMaintRestClient>(ServiceAddressConsts.MaintService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.WhseService, restPolicies);
        //rpc-event
        AddCapEventBus<CapEventSubscriber>();
    }
}