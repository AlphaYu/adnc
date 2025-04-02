using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Whse.Api;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration) : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    public override void AddAdncServices()
    {
        var registrar = new Application.DependencyRegistrar(Services, ServiceInfo, Configuration);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        Services.AddHealthChecks(checksBuilder =>
        {
            var connectionString = Configuration.GetValue<string>(NodeConsts.SqlServer_ConnectionString) ?? throw new ArgumentNullException(nameof(NodeConsts.SqlServer_ConnectionString));
            checksBuilder
                    .AddRedis(Configuration)
                    .AddRabbitMQ(Configuration, ServiceInfo.Id);
        });
    }
}
