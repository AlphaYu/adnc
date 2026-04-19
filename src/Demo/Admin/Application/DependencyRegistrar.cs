namespace Adnc.Demo.Admin.Application;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{
    //private readonly IServiceCollection _services = services;
    //private readonly IServiceInfo _serviceInfo = serviceInfo;
    //private readonly IConfiguration _configuration = configuration;

    protected override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

    protected override Assembly RepositoryOrDomainLayerAssembly => typeof(EntityInfo).Assembly;

    public override void AddApplicationServices()
    {
        AddApplicaitonDefaultServices();
        //add other serviceProvider
        //_services.Addxxxxx();
    }
}
