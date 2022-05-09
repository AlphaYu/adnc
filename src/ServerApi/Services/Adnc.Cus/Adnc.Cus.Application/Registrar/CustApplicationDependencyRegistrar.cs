using Adnc.Cus.Application.EventSubscribers;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.Rpc.Grpc.Services;
using Adnc.Shared.Rpc.Rest.Services;
using System.Reflection;

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
        var authRestAddress = IsDevelopment ? "http://localhost:50010" : "adnc.usr.webapi";
        var usrRestAddress = authRestAddress;
        var maintRestAddress = IsDevelopment ? "http://localhost:50020" : "adnc.maint.webapi";
        var whseRestAddress = IsDevelopment ? "http://localhost:50050" : "adnc.whse.webapi";
        AddRestClient<IAuthRestClient>(authRestAddress, restPolicies);
        AddRestClient<IUsrRestClient>(usrRestAddress, restPolicies);
        AddRestClient<IMaintRestClient>(maintRestAddress, restPolicies);
        AddRestClient<IWhseRestClient>(whseRestAddress, restPolicies);
        //rpc-grpcclient
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        var authGrpcAddress = IsDevelopment ? "http://localhost:50011" : "adnc.usr.webapi";
        var usrGrpcAddress = authGrpcAddress;
        var maintGrpcAddress = IsDevelopment ? "http://localhost:50021" : "adnc.maint.webapi";
        var whseGrpcAddress = IsDevelopment ? "http://localhost:50051" : "adnc.whse.webapi";
        AddGrpcClient<AuthGrpc.AuthGrpcClient>(authGrpcAddress, gprcPolicies);
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(usrGrpcAddress, gprcPolicies);
        AddGrpcClient<MaintGrpc.MaintGrpcClient>(maintGrpcAddress, gprcPolicies);
        AddGrpcClient<WhseGrpc.WhseGrpcClient>(whseGrpcAddress, gprcPolicies);
        //rpc-even
        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}