using Adnc.Maint.Application.AutoMapper;
using Adnc.Shared.Application.Registrar;
using FluentValidation;
using System.Reflection;

namespace Adnc.Maint.Application.Registrar;

public sealed class MaintApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsAssembly => typeof(IDictAppService).Assembly;

    public override Assembly RepositoryOrDomainAssembly => typeof(EntityInfo).Assembly;

    public MaintApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdncServices()
    {
        Services.AddValidatorsFromAssembly(ContractsAssembly, ServiceLifetime.Scoped);
        Services.AddAdncMapper(typeof(MaintProfile));
        AddApplicationSharedServices();
        AddConsulServices();
        AddCachingServices();
        AddBloomFilterServices();
        AddEfCoreContextWithRepositories<EntityInfo>();
        AddMongoContextWithRepositries();
        AddAppliactionSerivcesWithInterceptors();
        AddApplicaitonHostedServices();

        var policies = this.GenerateDefaultRefitPolicies();
        var authServeiceAddress = IsDevelopment ? "http://localhost:5010" : "adnc.usr.webapi";
        AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
        AddRpcService<IUsrRpcService>(authServeiceAddress, policies);
    }
}
