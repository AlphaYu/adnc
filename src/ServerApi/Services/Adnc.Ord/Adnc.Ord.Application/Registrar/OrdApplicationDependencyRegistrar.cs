using Adnc.Ord.Application.AutoMapper;
using Adnc.Ord.Application.EventSubscribers;
using Adnc.Ord.Domain.EntityConfig;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.Domain;
using FluentValidation;
using System.Reflection;

namespace Adnc.Ord.Application.Registrar;

public sealed class OrdApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsAssembly => typeof(IOrderAppService).Assembly;

    public override Assembly RepositoryOrDomainAssembly => typeof(EntityInfo).Assembly;

    public OrdApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        Services.AddValidatorsFromAssembly(ContractsAssembly, ServiceLifetime.Scoped);
        Services.AddAdncInfraAutoMapper(typeof(OrdProfile));
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
        AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
        var maintServiceAddress = IsDevelopment ? "http://localhost:5020" : "adnc.maint.webapi";
        AddRpcService<IMaintRpcService>(maintServiceAddress, policies);
        var whseServiceAddress = IsDevelopment ? "http://localhost:8065" : "adnc.whse.webapi";
        AddRpcService<IWhseRpcService>(whseServiceAddress, policies);

        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}
