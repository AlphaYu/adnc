﻿using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Demo.Ord.Api;

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
            checksBuilder
                    .AddMySql(Configuration)
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