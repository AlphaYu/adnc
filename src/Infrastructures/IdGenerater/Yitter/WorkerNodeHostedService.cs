namespace Adnc.Infra.IdGenerater.Yitter;

public sealed class WorkerNodeHostedService(ILogger<WorkerNodeHostedService> logger, WorkerNode workerNode) : BackgroundService
{
    private readonly int _millisecondsDelay = 1000 * 60;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await workerNode.InitWorkerNodesAsync();
        var workerId = await workerNode.GetWorkerIdAsync();

        IdGenerater.SetWorkerId((ushort)workerId);

        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        var nodeName = workerNode.GetWorkerNodeName();
        logger.LogInformation("stopping service {nodeName}", nodeName);

        var subtractionMilliseconds = 0 - (_millisecondsDelay * 1.5);
        var score = DateTime.Now.AddMilliseconds(subtractionMilliseconds).GetTotalMilliseconds();
        await workerNode.RefreshWorkerIdScoreAsync(IdGenerater.CurrentWorkerId, score);

        logger.LogInformation("stopped service {nodeName}:{score}", nodeName, score);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Factory.StartNew(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_millisecondsDelay, stoppingToken);

                    if (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }

                    await workerNode.RefreshWorkerIdScoreAsync(IdGenerater.CurrentWorkerId);
                }
                catch (Exception ex)
                {
                    var message = $"{nameof(ExecuteAsync)}:{workerNode.GetWorkerNodeName()}-{IdGenerater.CurrentWorkerId}";
                    logger.LogError(ex, "{message}", message);
                    await Task.Delay(_millisecondsDelay / 3, stoppingToken);
                }
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        await Task.CompletedTask;
    }
}
