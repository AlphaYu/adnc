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

    public WhseApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddApplicaitonDefault();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        var authRestAddress = IsDevelopment ? "http://localhost:50010" : "adnc.usr.webapi";
        AddRestClient<IAuthRestClient>(authRestAddress, restPolicies);
        var usrRestAddress = authRestAddress;
        AddRestClient<IUsrRestClient>(usrRestAddress, restPolicies);
        var maintRestAddress = IsDevelopment ? "http://localhost:50020" : "adnc.maint.webapi";
        AddRestClient<IMaintRestClient>(maintRestAddress, restPolicies);
        //rpc-event
        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}