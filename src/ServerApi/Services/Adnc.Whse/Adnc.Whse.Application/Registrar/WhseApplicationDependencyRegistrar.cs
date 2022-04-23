using Adnc.Shared.Application.Registrar;
using Adnc.Shared.Domain;
using Adnc.Whse.Application.AutoMapper;
using Adnc.Whse.Application.EventSubscribers;
using Adnc.Whse.Domain.EntityConfig;
using FluentValidation;
using System.Reflection;

namespace Adnc.Whse.Application.Registrar;

public sealed class WhseApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsAssembly => typeof(IWarehouseAppService).Assembly;

    public override Assembly RepositoryOrDomainAssembly => typeof(EntityInfo).Assembly;

    public WhseApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        Services.AddValidatorsFromAssembly(ContractsAssembly, ServiceLifetime.Scoped);
        Services.AddAdncInfraAutoMapper(typeof(WhseProfile));
        AddApplicationSharedServices();
        AddConsulServices();
        AddCachingServices();
        AddBloomFilterServices();
        AddEfCoreContextWithRepositories();
        AddMongoContextWithRepositries();
        AddAppliactionSerivcesWithInterceptors();
        AddApplicaitonHostedServices();
        AddDomainSerivces<IDomainService>();

        var policies = this.GenerateDefaultRefitPolicies();
        var authServeiceAddress = IsDevelopment ? "http://localhost:5010" : "adnc.usr.webapi";
        var maintServiceAddress = IsDevelopment ? "http://localhost:5020" : "adnc.maint.webapi";
        AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
        AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}
