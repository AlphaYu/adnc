using Adnc.Infra.Core.Interfaces;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.Rpc.Http.Services;
using Microsoft.Extensions.Configuration;

namespace Adnc.Demo.Whse.Application;

public sealed class DependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IWarehouseAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    private readonly IConfigurationSection _sqlSection;

    public DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo)
        : base(services, serviceInfo)
    {
        _sqlSection = Configuration.GetSection("SqlServer");
    }

    public override void AddApplicationServices()
    {
        AddApplicaitonDefault();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = PollyStrategyEnable ? this.GenerateDefaultRefitPolicies() : new();
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.AdncDemoAuthService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.AdncDemoUsrService, restPolicies);
        AddRestClient<IMaintRestClient>(ServiceAddressConsts.AdncDemoMaintService, restPolicies);
        //rpc-event
        AddCapEventBus<CapEventSubscriber>(replaceDbAction: capOption =>
        {
            var connectionString = _sqlSection.GetValue<string>("ConnectionString");
            capOption.UseSqlServer(config =>
            {
                config.ConnectionString = connectionString;
                config.Schema = "cap";
            });
        });
    }

    protected override void AddEfCoreContext() => Services.AddAdncInfraEfCoreSQLServer(_sqlSection);
}