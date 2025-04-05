using Adnc.Shared;
using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Application.Registrar;
using DotNetCore.CAP.Messages;
using Microsoft.Extensions.Configuration;

namespace Adnc.Demo.Whse.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
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
        AddCapEventBus([typeof(CapEventSubscriber)]);
    }

    protected override void AddCapEventBus(IEnumerable<Type> subscribers, Action<FailedInfo>? failedThresholdCallback = null)
    {
        var connectionString = Configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null");
        var rabbitMQOptions = Configuration.GetRequiredSection(NodeConsts.RabbitMq).Get<RabbitMQOptions>() ?? throw new InvalidDataException(nameof(NodeConsts.RabbitMq));
        var clientProvidedName = ServiceInfo.Id;
        var version = ServiceInfo.Version;
        var groupName = $"cap.{ServiceInfo.ShortName}.{this.GetEnvShortName()}";
        Services.AddAdncInfraCap(subscribers, capOptions =>
        {
            SetCapBasicInfo(capOptions, version, groupName, failedThresholdCallback);
            SetCapRabbitMQInfo(capOptions, rabbitMQOptions, clientProvidedName);
            capOptions.UseSqlServer(sqlServerOptions =>
            {
                sqlServerOptions.ConnectionString = connectionString;
                sqlServerOptions.Schema = "cap";
            });
        }, null, Lifetime);
    }

    protected override void AddEfCoreContext()
    {
        AddOperater(Services);

        var connectionString = Configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null");
        var migrationsAssemblyName = ServiceInfo.MigrationsAssemblyName;
        Services.AddAdncInfraEfCoreSQLServer(RepositoryOrDomainLayerAssembly, optionsBuilder =>
        {
            optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .MigrationsAssembly(migrationsAssemblyName)
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }, Lifetime);
    }
}
