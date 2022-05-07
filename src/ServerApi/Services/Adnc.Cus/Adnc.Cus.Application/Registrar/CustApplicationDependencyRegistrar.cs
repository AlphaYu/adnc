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

        //rpc-rest
        var policies = this.GenerateDefaultRefitPolicies();
        var authServeiceAddress = IsDevelopment ? "http://localhost:5010" : "adnc.usr.webapi";
        var maintServiceAddress = IsDevelopment ? "http://localhost:5020" : "adnc.maint.webapi";
        AddRestClient<IAuthRestClient>(authServeiceAddress, policies);
        AddRestClient<IMaintRestClient>(maintServiceAddress, policies);
        //rpc-grpc
        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        var authGrpcAddress = IsDevelopment ? "http://localhost:5011" : "adnc.usr.webapi";
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(authGrpcAddress, gprcPolicies);
     
        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}
