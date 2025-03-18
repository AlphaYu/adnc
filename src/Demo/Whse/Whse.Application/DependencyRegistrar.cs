using Adnc.Shared;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;
using Microsoft.Extensions.Configuration;

namespace Adnc.Demo.Whse.Application;

public sealed class DependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IWarehouseService).Assembly;

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
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
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

    protected override void AddEfCoreContext() => Services.AddAdncInfraEfCoreSQLServer(_sqlSection, ServiceInfo.MigrationsAssemblyName);
}