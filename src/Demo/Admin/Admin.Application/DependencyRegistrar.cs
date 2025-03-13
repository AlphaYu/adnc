namespace Adnc.Demo.Admin.Application;

public sealed class DependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => typeof(IUserService).Assembly;

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : base(services, serviceInfo)
    {
    }

    public override void AddApplicationServices()
    {
        AddApplicaitonDefault();
        //add other services
        //Services.Addxxxxx();
    }
}


