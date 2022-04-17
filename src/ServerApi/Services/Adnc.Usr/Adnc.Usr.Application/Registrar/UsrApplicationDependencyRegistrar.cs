using Adnc.Shared.Application.Registrar;
using Adnc.Usr.Application.AutoMapper;
using FluentValidation;

namespace Adnc.Usr.Application.Registrar;

public sealed class UsrApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsAssembly => typeof(IUserAppService).Assembly;

    public override Assembly RepositoryOrDomainAssembly => typeof(EntityInfo).Assembly;

    public UsrApplicationDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdncServices()
    {
        Services.AddValidatorsFromAssembly(ContractsAssembly, ServiceLifetime.Scoped);
        Services.AddAdncMapper(typeof(UsrProfile));
        AddApplicationSharedServices();
        AddConsulServices();
        AddCachingServices();
        AddBloomFilterServices();
        AddEfCoreContextWithRepositories<EntityInfo>();
        AddMongoContextWithRepositries();
        AddAppliactionSerivcesWithInterceptors();
        AddApplicaitonHostedServices();
    }
}
