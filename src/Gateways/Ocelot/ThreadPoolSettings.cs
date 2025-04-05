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
}
