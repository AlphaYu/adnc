using Adnc.Cus.Application.AutoMapper;
using Adnc.Cus.Application.EventSubscribers;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.RpcServices.Services;
using FluentValidation;
using System.Reflection;

namespace Adnc.Cus.Application.Registrar;

public sealed class CustApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsAssembly => typeof(ICustomerAppService).Assembly;

    public override Assembly RepositoryOrDomainAssembly => typeof(EntityInfo).Assembly;

    public CustApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        Services.AddValidatorsFromAssembly(ContractsAssembly, ServiceLifetime.Scoped);
        Services.AddAdncInfraAutoMapper(typeof(CustProfile));
        AddApplicationSharedServices();
        AddConsulServices();
        AddCachingServices();
        AddBloomFilterServices();
        AddDapperRepositories();
        AddEfCoreContextWithRepositories();
        AddMongoContextWithRepositries();
        AddAppliactionSerivcesWithInterceptors();
        AddApplicaitonHostedServices();

        var policies = this.GenerateDefaultRefitPolicies();
        var authServeiceAddress = IsDevelopment ? "http://localhost:5010" : "adnc.usr.webapi";
        var maintServiceAddress = IsDevelopment ? "http://localhost:5020" : "adnc.maint.webapi";
        AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
        AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

        AddEventBusPublishers();
        AddEventBusSubscribers<CapEventSubscriber>();
    }
}
