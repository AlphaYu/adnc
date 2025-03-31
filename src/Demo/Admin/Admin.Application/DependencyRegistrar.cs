namespace Adnc.Demo.Admin.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, lifetime)
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IUserService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefaultServices();
        //add other serviceProvider
        //Services.Addxxxxx();
    }
}
