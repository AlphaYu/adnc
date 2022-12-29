﻿namespace Adnc.Maint.Application;

public sealed class DependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IDictAppService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public DependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddApplicaitonDefault();
        //rpc-rest
        var restPolicies = this.GenerateDefaultRefitPolicies();
        AddRestClient<IAuthRestClient>(ServiceAddressConsts.UsrService, restPolicies);
        AddRestClient<IUsrRestClient>(ServiceAddressConsts.UsrService, restPolicies);

        AddRabbitMqClient();
    }
}