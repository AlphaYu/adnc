using Adnc.Shared.Remote;
using Adnc.Shared.WebApi;

namespace Microsoft.Extensions.Hosting;

public static class HostExtension
{
    /// <summary>
    /// register to (consul/nacos/clusterip...)
    /// </summary>
    public static IHost UseRegistrationCenter(this IHost host)
    {
        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var serviceInfo = host.Services.GetRequiredService<IServiceInfo>();
        var logger = host.Services.GetRequiredService<ILogger<RpcInfo>>();
        var registeredType = configuration.GetValue<string>($"{NodeConsts.RegisterType}") ?? RegisteredTypeConsts.Direct;
        logger.LogInformation("RegisteredType={registeredType}", registeredType);
        switch (registeredType)
        {
            case RegisteredTypeConsts.Consul:
                var kestrelSection = configuration.GetSection(NodeConsts.Kestrel);
                host.RegisterToConsul(serviceInfo.Id, kestrelSection);
                break;
            case RegisteredTypeConsts.Nacos:
                // TODO
                //app.RegisterToNacos(serviceInfo.Id);
                break;
            default:
                break;
        }
        return host;
    }

    public static IHost ChangeThreadPoolSettings(this IHost host)
    {
        //var poolOptions = host.Services.GetService(typeof(IOptionsMonitor<ThreadPoolSettings>)) as IOptionsMonitor<ThreadPoolSettings>;
        var poolOptions = host.Services.GetService(typeof(IOptions<ThreadPoolSettings>)) as IOptions<ThreadPoolSettings>;
        if (poolOptions is not null)
        {
            ChangeThreadPoolSettings(host, poolOptions);
        }

        return host;
    }

    public static IHost ChangeThreadPoolSettings(this IHost host, IOptions<ThreadPoolSettings> poolOptions)
    {
        var logger = host.Services.GetRequiredService<ILogger<IHost>>();
        var poolSetting = poolOptions.Value;
        ThreadPool.SetMinThreads(poolSetting.MinThreads, poolSetting.MinCompletionPortThreads);
        ThreadPool.SetMaxThreads(poolSetting.MaxThreads, poolSetting.MaxCompletionPortThreads);
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
        ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
        logger.LogInformation("Setting MinThreads={workerThreads},MinCompletionPortThreads={completionPortThreads}", workerThreads, completionPortThreads);
        logger.LogInformation("Setting MaxThreads={maxWorkerThreads},MaxCompletionPortThreads={maxCompletionPortThreads}", maxWorkerThreads, maxCompletionPortThreads);
        return host;
    }

    public static IHost ChangeThreadPoolSettings(this IHost host, IOptionsMonitor<ThreadPoolSettings> poolOptions)
    {
        var logger = host.Services.GetRequiredService<ILogger<IHost>>();
        poolOptions.OnChange(poolSetting =>
        {
            ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
            logger.LogInformation("before MinThreads={workerThreads},MinCompletionPortThreads={completionPortThreads}", workerThreads, completionPortThreads);
            logger.LogInformation("before MaxThreads={maxWorkerThreads},MaxCompletionPortThreads={maxCompletionPortThreads}", maxWorkerThreads, maxCompletionPortThreads);

            ThreadPool.SetMinThreads(poolSetting.MinThreads, poolSetting.MinCompletionPortThreads);
            ThreadPool.SetMaxThreads(poolSetting.MaxThreads, poolSetting.MaxCompletionPortThreads);

            ThreadPool.GetMinThreads(out var changedWorkerThreads, out var changedCompletionPortThreads);
            ThreadPool.GetMaxThreads(out var changedMaxWorkerThreads, out var changedsMaxCompletionPortThreads);
            logger.LogInformation("changed MinThreads={changedWorkerThreads},MinCompletionPortThreads={changedCompletionPortThreads}", changedWorkerThreads, changedCompletionPortThreads);
            logger.LogInformation("changed MaxThreads={changedMaxWorkerThreads},MaxCompletionPortThreads={changedsMaxCompletionPortThreads}", changedMaxWorkerThreads, changedsMaxCompletionPortThreads);
        });

        var poolSetting = poolOptions.CurrentValue;
        ThreadPool.SetMinThreads(poolSetting.MinThreads, poolSetting.MinCompletionPortThreads);
        ThreadPool.SetMaxThreads(poolSetting.MaxThreads, poolSetting.MaxCompletionPortThreads);

        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
        ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxCompletionPortThreads);
        logger.LogInformation("Setting MinThreads={workerThreads},MinCompletionPortThreads={completionPortThreads}", workerThreads, completionPortThreads);
        logger.LogInformation("Setting MaxThreads={maxWorkerThreads},MaxCompletionPortThreads={maxCompletionPortThreads}", maxWorkerThreads, maxCompletionPortThreads);
        return host;
    }
}
