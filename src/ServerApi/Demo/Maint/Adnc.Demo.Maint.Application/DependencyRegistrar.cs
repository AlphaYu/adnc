using Adnc.Demo.Maint.Repository;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Rpc.Http.Services;
using IUsrRestClient = Adnc.Demo.Shared.Rpc.Http.Services.IUsrRestClient;

namespace Adnc.Demo.Maint.Application;

public sealed class DependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : base(services, serviceInfo)
    {
    }

    public override void AddApplicationServices()
    {
        AddApplicaitonDefault();
        //rpc-rest
        var restPolicies = PollyStrategyEnable ? this.GenerateDefaultRefitPolicies() : new();
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.AdncDemoAuthService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.AdncDemoUsrService, restPolicies);

        AddRabbitMqClient();
    }
}