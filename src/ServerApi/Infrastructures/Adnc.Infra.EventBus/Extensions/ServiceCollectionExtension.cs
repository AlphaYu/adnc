using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.EventBus.RabbitMq;

namespace Microsoft.Extensions.DependencyInjection;

public static class AdncInfraEventBusServiceCollectionExtension
{
    public static IServiceCollection AddAdncEventBusPublishers(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMqProducer>();
        services.AddSingleton<IEventPublisher, CapPublisher>();
        return services;
    }
}