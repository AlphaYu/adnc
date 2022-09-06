using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    /// <summary>
    ///regiser Hangfire client and server
    /// </summary>
    /// <param name="builder"></param>
    //public static IServiceCollection AddAdncInfraHangfireWithServer(this IServiceCollection services,IConfigurationSection hangfireSection, Assembly assembly)
    //{
    //    if (services.HasRegistered(nameof(AddAdncInfraHangfireWithServer)) || hangfireSection is null)
    //        return services;

    //    var implTypes = assembly.GetImplementationTypesWithOutAbstractClass<IHangfireRecurringJob>();
    //    implTypes.ForEach(implType => services.AddScoped(implType));

    //    services.AddHangfire(configuration =>
    //    {
    //        var hangfireConfig = hangfireSection.Get<HangfireConfig>();
    //        var storageOptions = hangfireConfig.StorageOptions ?? new Hangfire.Redis.RedisStorageOptions();
    //        storageOptions.Prefix = storageOptions.Prefix.Replace(Hangfire.Redis.RedisStorageOptions.DefaultPrefix, "hangfire:");
    //        var redisConnector = ConnectionMultiplexer.Connect(hangfireConfig.ConnectionString);
    //        configuration.UseNLogLogProvider();
    //        configuration.UseRedisStorage(redisConnector, storageOptions);
    //    });

    //    return AddAdncInfraHangfireServer(services, hangfireSection);
    //}

    /// <summary>
    ///regiser Hangfire server
    /// </summary>
    /// <param name="builder"></param>
    //public static IServiceCollection AddAdncInfraHangfireServer(this IServiceCollection services, IConfigurationSection hangfireSection)
    //{
    //    if (services.HasRegistered(nameof(AddAdncInfraHangfireServer)) || hangfireSection is null)
    //        return services;

    //    return
    //        services.AddHangfireServer(serverOptions =>
    //        {
    //            var hangfireConfig = hangfireSection.Get<HangfireConfig>();
    //            var serviceInfo = services.GetServiceInfo();
    //            serverOptions.ServerName = serviceInfo.ShortName;
    //            serverOptions.Queues = new string[] { serviceInfo.ShortName };
    //            serverOptions.SchedulePollingInterval = TimeSpan.FromSeconds(15);
    //            serverOptions.HeartbeatInterval = TimeSpan.FromSeconds(30);
    //            serverOptions.ServerTimeout = TimeSpan.FromMinutes(5);
    //            serverOptions.ServerCheckInterval = TimeSpan.FromMinutes(5);
    //        });
    //}
}