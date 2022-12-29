namespace Adnc.Cust.Application;

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
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.UsrService, restPolicies);
        AddRestClient<IMaintRestClient>(ServiceAddressConsts.MaintService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.WhseService, restPolicies);
        //rpc-grpcclient
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AuthGrpc.AuthGrpcClient>(ServiceAddressConsts.UsrService, gprcPolicies);
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(ServiceAddressConsts.UsrService, gprcPolicies);
        AddGrpcClient<MaintGrpc.MaintGrpcClient>(ServiceAddressConsts.MaintService, gprcPolicies);
        AddGrpcClient<WhseGrpc.WhseGrpcClient>(ServiceAddressConsts.WhseService, gprcPolicies);
        //rpc-even
        AddCapEventBus<CapEventSubscriber>();
    }
}