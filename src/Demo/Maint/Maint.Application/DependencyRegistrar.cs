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
        // AddRabbitMqClient();
    }
}