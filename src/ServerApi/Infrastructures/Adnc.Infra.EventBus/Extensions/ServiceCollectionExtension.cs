using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.EventBus.RabbitMq;
using DotNetCore.CAP;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{

    public static IServiceCollection AddAdncInfraCap<TSubscriber>(this IServiceCollection services, Action<CapOptions> setupAction)
        where TSubscriber : class, ICapSubscribe
    {
        if (services.HasRegistered(nameof(AddAdncInfraCap)))
            return services;
        services
            .AddSingleton<IEventPublisher, CapPublisher>()
            .AddSingleton<TSubscriber>()
            .AddCap(setupAction)
            ;
        return services;
    }


    public static IServiceCollection AddAdncInfraRabbitMq(this IServiceCollection services, IConfigurationSection rabitmqSection)
    {
        if (services.HasRegistered(nameof(AddAdncInfraRabbitMq)))
            return services;

        return services
             .Configure<RabbitMqConfig>(rabitmqSection)
             .AddSingleton<IRabbitMqConnection>(provider =>
             {
                 var options = provider.GetRequiredService<IOptions<RabbitMqConfig>>();
                 var logger = provider.GetRequiredService<ILogger<RabbitMqConnection>>();
                 var serviceInfo = services.GetServiceInfo();
                 var clientProvidedName = serviceInfo?.Id ?? "unkonow";
                 return RabbitMqConnection.GetInstance(options, clientProvidedName, logger);
             })
             .AddSingleton<RabbitMqProducer>()
             ;
    }
}