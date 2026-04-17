using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Whse.Api;

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

        var connectionString = _configuration.GetValue<string>(NodeConsts.SqlServer_ConnectionString) ?? throw new ArgumentNullException(nameof(NodeConsts.SqlServer_ConnectionString));
        var redisSecton = _configuration.GetRequiredSection(NodeConsts.Redis);
        var rabbitSecton = _configuration.GetRequiredSection(NodeConsts.RabbitMq);
        var clientProvidedName = _serviceInfo.Id;
        _services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddSqlServer(connectionString)
                    .AddRedis(redisSecton)
                    .AddRabbitMQ(rabbitSecton, clientProvidedName);
        });
    }
}
