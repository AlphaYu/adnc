using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.EventBus.RabbitMq;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraEventBus(this IServiceCollection services, IConfigurationSection consulSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraEventBus)))
            return services;

        return services
             .Configure<RabbitMqConfig>(consulSection)
             .AddSingleton<IRabbitMqConnection>(provider =>
             {
                 var options = provider.GetRequiredService<IOptions<RabbitMqConfig>>();
                 var logger = provider.GetRequiredService<ILogger<RabbitMqConnection>>();
                 return RabbitMqConnection.GetInstance(options, logger);
             })
             .AddSingleton<RabbitMqProducer>()
             .AddSingleton<IEventPublisher, CapPublisher>()
             ;
    }
}