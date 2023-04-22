using Adnc.Demo.Cust.Api.Application.Subscribers;
using Adnc.Demo.Shared.Const;
using Adnc.Shared.Application.Registrar;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Cust.Api;

public sealed class ApiLayerRegistrar : AbstractWebApiDependencyRegistrar
{
    public ApiLayerRegistrar(IServiceCollection services) : base(services)
    {
    }

    public ApiLayerRegistrar(IApplicationBuilder appBuilder) : base(appBuilder)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddHealthChecks(true, true, true, false);
        //register others services
        //Services.AddScoped<xxxx>
    }

    public override void UseAdnc()
    {
        UseWebApiDefault();
    }
}

public sealed class ApplicationLayerRegistrar : AbstractApplicationDependencyRegistrar
{
    private readonly Assembly _assembly;

    public ApplicationLayerRegistrar(IServiceCollection services) : base(services)
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public override Assembly ApplicationLayerAssembly => _assembly;
    public override Assembly ContractsLayerAssembly => _assembly;
    public override Assembly RepositoryOrDomainLayerAssembly => _assembly;

    public override void AddAdnc()
    {
        //register base services
        AddApplicaitonDefault();
        //register rpc-http services
        var restPolicies = PollyStrategyEnable ? this.GenerateDefaultRefitPolicies() : new();
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.AdncDemoAuthService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.AdncDemoUsrService, restPolicies);
        AddRestClient<IMaintRestClient>(ServiceAddressConsts.AdncDemoMaintService, restPolicies);
        AddRestClient<IWhseRestClient>(ServiceAddressConsts.AdncDemoWhseService, restPolicies);

        var gprcPolicies = this.GenerateDefaultGrpcPolicies();
        AddGrpcClient<AuthGrpc.AuthGrpcClient>(ServiceAddressConsts.AdncDemoAuthService, gprcPolicies);
        AddGrpcClient<UsrGrpc.UsrGrpcClient>(ServiceAddressConsts.AdncDemoUsrService, gprcPolicies);
        AddGrpcClient<MaintGrpc.MaintGrpcClient>(ServiceAddressConsts.AdncDemoMaintService, gprcPolicies);
        AddGrpcClient<WhseGrpc.WhseGrpcClient>(ServiceAddressConsts.AdncDemoWhseService, gprcPolicies);
        //register rpc-event service
        AddCapEventBus<CapEventSubscriber>();
        //register others services
        //Services.AddScoped<xxxx>
    }
}