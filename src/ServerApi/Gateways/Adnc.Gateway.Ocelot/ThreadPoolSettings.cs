using Microsoft.Extensions.Options;

namespace Adnc.Gateway.Ocelot;

public sealed class ThreadPoolSettings
{
    public int MinThreads { get; set; } = 300;
    public int MinCompletionPortThreads { get; set; } = 300;
    public int MaxThreads { get; set; } = 32767;
    public int MaxCompletionPortThreads { get; set; } = 1000;
}

public static class HostExtensions
{
    public static IHost ChangeThreadPoolSettings(this IHost host)
    {
        //var poolOptions = host.Services.GetService(typeof(IOptionsMonitor<ThreadPoolSettings>)) as IOptionsMonitor<ThreadPoolSettings>;
        var poolOptions = host.Services.GetService(typeof(IOptions<ThreadPoolSettings>)) as IOptions<ThreadPoolSettings>;
        if (poolOptions is not null)
            ChangeThreadPoolSettings(host, poolOptions);
        return host;
    }

    public static IHost ChangeThreadPoolSettings(this IHost host, IOptions<ThreadPoolSettings> poolOptions)
    {
        if (host.Services.GetService(typeof(ILogger<IHost>)) is not ILogger<IHost> logger)
            throw new NullReferenceException(nameof(logger));

        var poolSetting = poolOptions.Value;
        ThreadPool.SetMinThreads(poolSetting.MinThreads, poolSetting.MinCompletionPortThreads);
        ThreadPool.SetMaxThreads(poolSetting.MaxThreads, poolSetting.MaxCompletionPortThreads);
        ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
        ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
        logger.LogInformation("Setting MinThreads={0},MinCompletionPortThreads={1}", workerThreads, completionPortThreads);
        logger.LogInformation("Setting MaxThreads={0},MaxCompletionPortThreads={1}", maxWorkerThreads, maxCompletionPortThreads);
        return host;
    }
}