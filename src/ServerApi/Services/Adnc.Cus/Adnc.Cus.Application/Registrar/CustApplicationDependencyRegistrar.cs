using Adnc.Shared.Application.Registrar;

namespace Adnc.Cus.Application.Registrar;

public sealed class CustApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(ICustomerAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public CustApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddApplicaitonDefault();

        //rpc-restclient
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAuthRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IMaintRestClient>(RpcConsts.MaintService, restPolicies);
        AddRestClient<IWhseRestClient>(RpcConsts.WhseService, restPolicies);
        //rpc-grpcclient
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AuthGrpc.AuthGrpcClient>(RpcConsts.UsrService, gprcPolicies);
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(RpcConsts.UsrService, gprcPolicies);
        AddGrpcClient<MaintGrpc.MaintGrpcClient>(RpcConsts.MaintService, gprcPolicies);
        AddGrpcClient<WhseGrpc.WhseGrpcClient>(RpcConsts.WhseService, gprcPolicies);
        //rpc-even
        AddCapEventBus<CapEventSubscriber>();
    }
}