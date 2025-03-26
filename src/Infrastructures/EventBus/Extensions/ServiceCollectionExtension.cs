using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.EventBus.Cap.Filters;
using Adnc.Infra.EventBus.Configurations;
using Adnc.Infra.EventBus.RabbitMq;
using DotNetCore.CAP;
using DotNetCore.CAP.Filter;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraCap<TSubscriber>(this IServiceCollection services, Action<CapOptions> setupAction)
        where TSubscriber : class, ICapSubscribe
    {
        return services.AddAdncInfraCap<TSubscriber, DefaultCapFilter>(setupAction);
    }

    public static IServiceCollection AddAdncInfraCap<TSubscriber, TSubscribeFilter>(this IServiceCollection services, Action<CapOptions> setupAction)
    where TSubscriber : class, ICapSubscribe
    where TSubscribeFilter : class, ISubscribeFilter
    {
        if (services.HasRegistered(nameof(AddAdncInfraCap)))
        {
            return services;
        }

        services
            .AddSingleton<IEventPublisher, CapPublisher>()
            .AddScoped<TSubscriber>()
            .AddCap(setupAction)
            .AddSubscribeFilter<TSubscribeFilter>()
            ;
        return services;
    }

    public static IServiceCollection AddAdncInfraRabbitMq(this IServiceCollection services, IConfigurationSection rabitmqSection, string clientProvidedName)
    {
        if (services.HasRegistered(nameof(AddAdncInfraRabbitMq)))
        {
            return services;
        }

        return services
             .Configure<RabbitMqOptions>(rabitmqSection)
             .AddSingleton<IRabbitMqConnection>(provider =>
             {
                 var options = provider.GetRequiredService<IOptions<RabbitMqOptions>>();
                 var logger = provider.GetRequiredService<ILogger<RabbitMqConnection>>();
                 var clientName = clientProvidedName.IsNullOrWhiteSpace() ? clientProvidedName : "unknow";
                 return RabbitMqConnection.GetInstance(options, clientName, logger);
             })
             .AddSingleton<RabbitMqProducer>()
             ;
    }
}