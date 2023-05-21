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
            using var scope = _services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LoginLog>>();
            var channelLoginReader = ChannelAccessor<LoginLog>.Instance.Reader;
            var maxAllowInsert = 100;
            var entities = new List<LoginLog>();
            while (await channelLoginReader.WaitToReadAsync(stoppingToken))
            {
                var currentExistsCount = channelLoginReader.Count;
                for (int index = 1; index <= currentExistsCount; index++)
                {
                    var entity = await channelLoginReader.ReadAsync();
                    entities.Add(entity);
                    if (index % maxAllowInsert == 0 || index == currentExistsCount)
                    {
                        try
                        {
                            await repository.AddManyAsync(entities);
                        }
                        catch (Exception ex)
                        {
                            var message = $"{nameof(ExecuteAsync)}:{nameof(channelLoginReader)}";
                            _logger.LogError(ex, message);
                        }
                        finally
                        {
                            entities.Clear();
                        }
                    }
                }

                if (stoppingToken.IsCancellationRequested) break;
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        //save operationlogs
        _ = Task.Factory.StartNew(async () =>
        {
            using var scope = _services.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<OperationLog>>();
            var channelOperationLogReader = ChannelAccessor<OperationLog>.Instance.Reader;
            var maxAllowInsert = 100;
            var entities = new List<OperationLog>();
            while (await channelOperationLogReader.WaitToReadAsync())
            {
                var currentExistsCount = channelOperationLogReader.Count;
                for (int index = 1; index <= currentExistsCount; index++)
                {
                    var entity = await channelOperationLogReader.ReadAsync();
                    entities.Add(entity);
                    if (index % maxAllowInsert == 0 || index == currentExistsCount)
                    {
                        try
                        {
                            await repository.AddManyAsync(entities);
                        }
                        catch (Exception ex)
                        {
                            var message = $"{nameof(ExecuteAsync)}:{nameof(channelOperationLogReader)}";
                            _logger.LogError(ex, message);
                        }
                        finally
                        {
                            entities.Clear();
                        }
                    }
                }

                if (stoppingToken.IsCancellationRequested) break;
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        await Task.CompletedTask;
    }
}