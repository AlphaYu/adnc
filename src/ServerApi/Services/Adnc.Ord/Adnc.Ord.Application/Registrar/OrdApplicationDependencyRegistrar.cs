using Adnc.Ord.Application.EventSubscribers;
using Adnc.Ord.Domain.EntityConfig;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.Domain;
using System.Reflection;

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
        var authRestAddress = IsDevelopment ? "http://localhost:50010" : "adnc.usr.webapi";
        AddRestClient<IAuthRestClient>(authRestAddress, restPolicies);
        var usrRestAddress = authRestAddress;
        AddRestClient<IUsrRestClient>(usrRestAddress, restPolicies);
        var maintRestAddress = IsDevelopment ? "http://localhost:50020" : "adnc.maint.webapi";
        AddRestClient<IMaintRestClient>(maintRestAddress, restPolicies);
        var whseRestAddress = IsDevelopment ? "http://localhost:50050" : "adnc.whse.webapi";
        AddRestClient<IWhseRestClient>(whseRestAddress, restPolicies);
        //rpc-event
        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}