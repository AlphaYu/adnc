using Adnc.Shared.Application.Registrar;
using Adnc.Shared.Domain;
using Adnc.Whse.Application.EventSubscribers;
using Adnc.Whse.Domain.EntityConfig;
using System.Reflection;

namespace Adnc.Whse.Application.Registrar;

public sealed class WhseApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IWarehouseAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public WhseApplicationDependencyRegistrar(IServiceCollection services) : base(services) { }

    public override void AddAdnc()
    {
        AddApplicaitonDefault();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        var authRestAddress = IsDevelopment ? "http://localhost:50010" : "adnc.usr.webapi";
        AddRestClient<IAuthRestClient>(authRestAddress, restPolicies);
        var maintRestAddress = IsDevelopment ? "http://localhost:50020" : "adnc.maint.webapi";
        AddRestClient<IMaintRestClient>(maintRestAddress, restPolicies);
        //rpc-grpc
        var grpcPolicies = this.GenerateDefaultGrpcPolicies();
        var usrGrpcAddress = IsDevelopment ? "http://localhost:50011" : "adnc.usr.webapi";
        AddGrpcClient<Adnc.Shared.Rpc.Grpc.Services.UsrGrpc.UsrGrpcClient>(usrGrpcAddress, grpcPolicies);
        //rpc-event
        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}
