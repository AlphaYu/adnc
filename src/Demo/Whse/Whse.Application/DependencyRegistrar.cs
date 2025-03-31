using Adnc.Shared;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;

namespace Adnc.Demo.Whse.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, lifetime)
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IWarehouseService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefaultServices();

        AddDomainSerivces<IDomainService>();
        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies);
        //rpc-event
        AddCapEventBus<CapEventSubscriber>(replaceDbAction: capOptions =>
        {
            var connectionString = Configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null"); ;
            capOptions.UseSqlServer(sqlServerOptions =>
            {
                sqlServerOptions.ConnectionString = connectionString;
                sqlServerOptions.Schema = "cap";
            });
        });
    }

    protected override void AddEfCoreContext()
    {
        AddOperater(Services);

        Services.AddAdncInfraEfCoreSQLServer(RepositoryOrDomainLayerAssembly, optionsBuilder =>
        {
            var connectionString = Configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null"); ;
            optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .MigrationsAssembly(ServiceInfo.MigrationsAssemblyName)
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }, Lifetime);
    }
}
