using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.EventBus.Cap.Filters;
using Adnc.Infra.EventBus.Configurations;
using Adnc.Infra.EventBus.RabbitMq;
using DotNetCore.CAP;
using DotNetCore.CAP.Filter;
using SkyApm.Diagnostics.CAP;
using SkyApm.Utilities.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraCap<TSubscriber>(this IServiceCollection services, Action<CapOptions> setupAction, Action<IServiceCollection>? registrarAction = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TSubscriber : class, ICapSubscribe
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));

        return services.AddAdncInfraCap<TSubscriber, DefaultCapFilter>(setupAction, registrarAction, serviceLifetime);
    }

    public static IServiceCollection AddAdncInfraCap<TSubscriber, TSubscribeFilter>(this IServiceCollection services, Action<CapOptions> setupAction, Action<IServiceCollection>? registrarAction = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    where TSubscriber : class, ICapSubscribe
    where TSubscribeFilter : class, ISubscribeFilter
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(setupAction, nameof(setupAction));

        if (services.HasRegistered(nameof(AddAdncInfraCap)))
        {
            return services;
        }

        registrarAction?.Invoke(services);

        if (IsEnableSkyApm())
        {
            services.AddSkyApmExtensions().AddCap();
        }

        services.Add(new ServiceDescriptor(typeof(TSubscriber), typeof(TSubscriber), serviceLifetime));

        services
            .AddSingleton<IEventPublisher, CapPublisher>()
            .AddCap(setupAction)
            .AddSubscribeFilter<TSubscribeFilter>();

        return services;
    }

    public static IServiceCollection AddAdncInfraRabbitMq(this IServiceCollection services, IConfigurationSection rabitmqSection, string clientProvidedName, Action<IServiceCollection>? registrarAction = null)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(rabitmqSection, nameof(rabitmqSection));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(clientProvidedName, nameof(clientProvidedName));

        if (services.HasRegistered(nameof(AddAdncInfraRabbitMq)))
        {
            return services;
        }

        registrarAction?.Invoke(services);

        return services
             .Configure<RabbitMqOptions>(rabitmqSection)
             .AddSingleton<IConnectionManager>(provider =>
             {
                 var options = provider.GetRequiredService<IOptions<RabbitMqOptions>>();
                 var logger = provider.GetRequiredService<ILogger<ConnectionManager>>();
                 var clientName = clientProvidedName.IsNullOrWhiteSpace() ? clientProvidedName : "unknow";
                 return ConnectionManager.GetInstance(options, clientName, logger);
             })
             .AddSingleton<RabbitMqProducer>();
    }

    public static bool IsEnableSkyApm()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES");
        if (string.IsNullOrWhiteSpace(env))
        {
            return false;
        }
        else
        {
            return env.Contains("SkyAPM.Agent.AspNetCore");
        }
    }
}
