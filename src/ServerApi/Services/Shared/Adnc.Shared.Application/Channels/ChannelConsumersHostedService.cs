namespace Adnc.Shared.Application.Channels;

public class ChannelConsumersHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ChannelConsumersHostedService> _logger;

    public ChannelConsumersHostedService(
       ILogger<ChannelConsumersHostedService> logger,
       IServiceProvider services)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //save loginlogs
        _ = Task.Factory.StartNew(async () =>
        {
            var channelLoginReader = ChannelHelper<LoginLog>.Instance.Reader;
            while (await channelLoginReader.WaitToReadAsync(stoppingToken))
            {
                if (channelLoginReader.TryRead(out var entity))
                {
                    using var scope = _services.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LoginLog>>();
                    try
                    {
                        await repository.AddAsync(entity, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        var message = $"{nameof(ExecuteAsync)}:{nameof(channelLoginReader)}";
                        _logger.LogError(ex, message);
                    }
                }
                if (stoppingToken.IsCancellationRequested) break;
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        //save operationlogs
        _ = Task.Factory.StartNew(async () =>
        {
            var channelOperationLogReader = ChannelHelper<OperationLog>.Instance.Reader;
            while (await channelOperationLogReader.WaitToReadAsync(stoppingToken))
            {
                if (channelOperationLogReader.TryRead(out var entity))
                {
                    using var scope = _services.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<OperationLog>>();
                    try
                    {
                        await repository.AddAsync(entity, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        var message = $"{nameof(ExecuteAsync)}:{nameof(channelOperationLogReader)}";
                        _logger.LogError(ex, message);
                    }
                }
                if (stoppingToken.IsCancellationRequested) break;
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        await Task.CompletedTask;
    }
}