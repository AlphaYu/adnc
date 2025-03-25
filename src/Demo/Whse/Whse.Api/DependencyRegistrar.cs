using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Whse.Api;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo) : AbstractWebApiDependencyRegistrar(services, serviceInfo)
{
    public override void AddAdncServices()
    {
        Services.AddSingleton(typeof(IServiceInfo), ServiceInfo);

        var registrar = new Application.DependencyRegistrar(Services, ServiceInfo);
        registrar.AddApplicationServices();

        AddWebApiDefaultServices();

        Services.AddHealthChecks(checksBuilder =>
        {
            var connectionString = Configuration.GetValue<string>(NodeConsts.SqlServer_ConnectionString) ?? throw new NullReferenceException(nameof(NodeConsts.SqlServer_ConnectionString));
            checksBuilder
                    .AddSqlServer(connectionString)
                    .AddRedis(Configuration)
                    .AddRabbitMQ(Configuration, ServiceInfo.Id);
        });
    }
}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdnc(this IServiceCollection services, IServiceInfo serviceInfo)
    {
        var registrar = new DependencyRegistrar(services, serviceInfo);
        registrar.AddAdncServices();
        return services;
    }
}