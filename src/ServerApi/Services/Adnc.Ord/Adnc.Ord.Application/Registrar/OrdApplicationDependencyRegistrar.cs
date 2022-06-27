using Adnc.Shared.Application.Registrar;

namespace Adnc.Ord.Application.Registrar;

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
        AddRestClient<IAuthRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IMaintRestClient>(RpcConsts.MaintService, restPolicies);
        AddRestClient<IWhseRestClient>(RpcConsts.WhseService, restPolicies);
        //rpc-event
        AddCapEventBus<CapEventSubscriber>();
    }
}