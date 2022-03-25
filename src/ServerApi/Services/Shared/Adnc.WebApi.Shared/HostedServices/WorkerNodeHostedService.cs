using Adnc.Application.Shared.IdGenerater;
using Adnc.Infra.Helper.IdGeneraterInternal;

namespace Adnc.WebApi.Shared.HostedServices;

public class WorkerNodeHostedService : BackgroundService
{
    private readonly ILogger<WorkerNodeHostedService> _logger;
    private readonly string _serviceName;
    private readonly WorkerNode _workerNode;
    private readonly int _millisecondsDelay = 1000 * 60;

    public WorkerNodeHostedService(ILogger<WorkerNodeHostedService> logger
       , WorkerNode workerNode
       , string serviceName)
    {
        _serviceName = serviceName;
        _workerNode = workerNode;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _workerNode.InitWorkerNodesAsync(_serviceName);
        var workerId = await _workerNode.GetWorkerIdAsync(_serviceName);

        YitterSnowFlake.CurrentWorkerId = (short)workerId;

        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        _logger.LogInformation("stopping service {0}", _serviceName);

        var subtractionMilliseconds = 0 - (_millisecondsDelay * 1.5);
        var score = DateTime.Now.AddMilliseconds(subtractionMilliseconds).GetTotalMilliseconds();
        await _workerNode.RefreshWorkerIdScoreAsync(_serviceName, YitterSnowFlake.CurrentWorkerId, score);

        _logger.LogInformation("stopped service {0}:{1}", _serviceName, score);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_millisecondsDelay, stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;

                await _workerNode.RefreshWorkerIdScoreAsync(_serviceName, YitterSnowFlake.CurrentWorkerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                await Task.Delay(_millisecondsDelay / 3, stoppingToken);
            }
        }
    }
}