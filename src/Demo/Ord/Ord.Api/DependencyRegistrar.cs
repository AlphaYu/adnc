using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Ord.Api;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration) : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    public override void AddAdncServices()
    {
        var registrar = new Application.DependencyRegistrar(Services, ServiceInfo, Configuration);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        var mysqlSection = Configuration.GetRequiredSection(NodeConsts.Mysql);
        var redisSecton = Configuration.GetRequiredSection(NodeConsts.Redis);
        var rabbitSecton = Configuration.GetRequiredSection(NodeConsts.RabbitMq);
        var clientProvidedName = ServiceInfo.Id;
        Services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddMySql(mysqlSection)
                    .AddRedis(redisSecton)
                    .AddRabbitMQ(rabbitSecton, clientProvidedName);
        });
    }
}
