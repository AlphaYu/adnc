﻿using Adnc.Shared.Application.Extensions;
using Adnc.Shared.Remote.Http.Services;
using IUsrRestClient = Adnc.Demo.Shared.Remote.Http.Services.IUsrRestClient;

namespace Adnc.Demo.Maint.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo) 
    : AbstractApplicationDependencyRegistrar(services, serviceInfo)
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

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