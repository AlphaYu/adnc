namespace Adnc.Demo.Admin.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : AbstractApplicationDependencyRegistrar(services, serviceInfo)
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IUserService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefault();
        //add other serviceProvider
        //Services.Addxxxxx();
    }
}

