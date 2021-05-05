using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Adnc.Infra.Common.Helper.IdGeneraterInternal;
using Adnc.Infra.Common.Extensions;

namespace Adnc.Application.Shared.IdGeneraterWorkerNode
{
    public class WorkerNodeHostedService : BackgroundService
    {
        private readonly ILogger<WorkerNodeHostedService> _logger;
        private readonly string  _serviceName;
        private readonly WorkerNode _workerNode;

        public WorkerNodeHostedService(ILogger<WorkerNodeHostedService> logger
           , WorkerNode workerNode
           , string serviceName)
        {
            _serviceName = serviceName;
            _workerNode = workerNode;
            _logger = logger;
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            await _workerNode.InitWorkerNodesAsync(_serviceName);
            var workerId = await _workerNode.GetWorkerIdAsync(_serviceName);

            YitterSnowFlake.CurrentWorkerId = (short)workerId;

            await base.StartAsync(cancellationToken);
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _logger.LogInformation("stopping....{0}", _serviceName);
            var score = DateTime.Now.AddMilliseconds(-(1000 * 90)).GetTotalMilliseconds();
            await _workerNode.RefreshWorkerIdScoreAsync(_serviceName, YitterSnowFlake.CurrentWorkerId, score);
            _logger.LogInformation("stopped....{0}:{1}", _serviceName, score);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    await Task.Delay(1000 * 60);
                    await _workerNode.RefreshWorkerIdScoreAsync(_serviceName, YitterSnowFlake.CurrentWorkerId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    await Task.Delay(1000 * 10);
                }
            }
        }
    }
}
