using Adnc.Shared.Application.Registrar;
using Microsoft.Extensions.Configuration;

namespace Adnc.Whse.Application.Registrar;

public sealed class WhseApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IWarehouseAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    private readonly IConfigurationSection _sqlSection;

    public WhseApplicationDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
        _sqlSection = Configuration.GetSection("SqlServer");
    }

    public override void AddAdnc()
    {
        AddApplicaitonDefault();
        AddDomainSerivces<IDomainService>();

        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAuthRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(RpcConsts.UsrService, restPolicies);
        AddRestClient<IMaintRestClient>(RpcConsts.MaintService, restPolicies);
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