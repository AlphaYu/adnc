namespace Adnc.Shared.Application.Channels;

public class ChannelConsumersHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ChannelConsumersHostedService> _logger;

    public ChannelConsumersHostedService(ILogger<ChannelConsumersHostedService> logger
        , IServiceProvider services)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //save loginlogs
        _ = Task.Run(async () =>
        {
            var channelLoginReader = ChannelHelper<LoginLog>.Instance.Reader;
            while (await channelLoginReader.WaitToReadAsync(stoppingToken))
            {
                if (channelLoginReader.TryRead(out var entity))
                {
                    using var scope = _services.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LoginLog>>();
                    await repository.AddAsync(entity, stoppingToken);
                }
                if (stoppingToken.IsCancellationRequested) break;
            }
        }, stoppingToken);

        //save operationlogs
        _ = Task.Run(async () =>
        {
            var channelOperationLogReader = ChannelHelper<OperationLog>.Instance.Reader;
            while (await channelOperationLogReader.WaitToReadAsync(stoppingToken))
            {
                if (channelOperationLogReader.TryRead(out var entity))
                {
                    using var scope = _services.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<OperationLog>>();
                    await repository.AddAsync(entity, stoppingToken);
                }
                if (stoppingToken.IsCancellationRequested) break;
            }
        }, stoppingToken);

        await Task.CompletedTask;
    }
}