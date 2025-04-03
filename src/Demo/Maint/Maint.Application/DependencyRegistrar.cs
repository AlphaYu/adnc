using Microsoft.Extensions.Configuration;

namespace Adnc.Demo.Maint.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly ContractsLayerAssembly => Assembly.GetExecutingAssembly();

    public override Assembly RepositoryOrDomainLayerAssembly => typeof(RepositoryExtension).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefaultServices();
        // AddRabbitMqClient();
    }
}
