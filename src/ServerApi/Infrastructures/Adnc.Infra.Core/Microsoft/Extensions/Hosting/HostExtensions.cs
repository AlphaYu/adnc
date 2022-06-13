namespace Microsoft.Extensions.Hosting
{
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

        public static IHost ChangeThreadPoolSettings(this IHost host, IOptionsMonitor<ThreadPoolSettings> poolOptions)
        {
            if (host.Services.GetService(typeof(ILogger<IHost>)) is not ILogger<IHost> logger)
                throw new NullReferenceException(nameof(logger));

            poolOptions.OnChange(poolSetting =>
            {
                ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
                ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
                logger.LogInformation("before MinThreads={0},MinCompletionPortThreads={1}", workerThreads, completionPortThreads);
                logger.LogInformation("before MaxThreads={0},MaxCompletionPortThreads={1}", maxWorkerThreads, maxCompletionPortThreads);

                ThreadPool.SetMinThreads(poolSetting.MinThreads, poolSetting.MinCompletionPortThreads);
                ThreadPool.SetMaxThreads(poolSetting.MaxThreads, poolSetting.MaxCompletionPortThreads);

                ThreadPool.GetMinThreads(out int changedWorkerThreads, out int changedCompletionPortThreads);
                ThreadPool.GetMaxThreads(out int changedMaxWorkerThreads, out int changedsMaxCompletionPortThreads);
                logger.LogInformation("changed MinThreads={0},MinCompletionPortThreads={1}", changedWorkerThreads, changedCompletionPortThreads);
                logger.LogInformation("changed MaxThreads={0},MaxCompletionPortThreads={1}", changedMaxWorkerThreads, changedsMaxCompletionPortThreads);
            });

            var poolSetting = poolOptions.CurrentValue;
            ThreadPool.SetMinThreads(poolSetting.MinThreads, poolSetting.MinCompletionPortThreads);
            ThreadPool.SetMaxThreads(poolSetting.MaxThreads, poolSetting.MaxCompletionPortThreads);

            ThreadPool.GetMinThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
            logger.LogInformation("Setting MinThreads={0},MinCompletionPortThreads={1}", workerThreads, completionPortThreads);
            logger.LogInformation("Setting MaxThreads={0},MaxCompletionPortThreads={1}", maxWorkerThreads, maxCompletionPortThreads);
            return host;
        }
    }
}