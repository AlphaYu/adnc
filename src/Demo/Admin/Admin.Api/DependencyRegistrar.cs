using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Admin.Api;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration) : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    private readonly IServiceCollection _services = services;
    private readonly IServiceInfo _serviceInfo = serviceInfo;
    private readonly IConfiguration _configuration = configuration;

    public override void AddAdncServices()
    {
        var registrar = new Application.DependencyRegistrar(_services, _serviceInfo, _configuration);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        var mysqlSection = _configuration.GetRequiredSection(NodeConsts.Mysql);
        var redisSecton = _configuration.GetRequiredSection(NodeConsts.Redis);
        var rabbitSecton = _configuration.GetRequiredSection(NodeConsts.RabbitMq);
        var clientProvidedName = _serviceInfo.Id;
        _services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddMySql(mysqlSection)
                    .AddRedis(redisSecton)
                    .AddRabbitMQ(rabbitSecton, clientProvidedName);
        });

        _services.AddGrpc();
    }
}
