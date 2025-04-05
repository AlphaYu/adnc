namespace Adnc.Shared.Application.Channels;

public class LogConsumersHostedService(ILogger<LogConsumersHostedService> logger, IServiceProvider services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var (connectionString, dbType) = configuration.GetDbConnectionInfo(NodeConsts.SysLogDb);

        //save loginlogs
        _ = Task.Factory.StartNew(async () =>
        {
            //using var scope = services.CreateScope();
            //var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LoginLog>>();
            var channelLoginReader = ChannelAccessor<LoginLog>.Instance.Reader;
            var maxAllowInsert = 100;
            var entities = new List<LoginLog>();
            while (await channelLoginReader.WaitToReadAsync(stoppingToken))
            {
                var currentExistsCount = channelLoginReader.Count;
                for (var index = 1; index <= currentExistsCount; index++)
                {
                    var entity = await channelLoginReader.ReadAsync();
                    entities.Add(entity);
                    if (index % maxAllowInsert == 0 || index == currentExistsCount)
                    {
                        try
                        {
                            //await repository.AddManyAsync(entities);
                            using var scope = services.CreateScope();
                            var repository = scope.ServiceProvider.GetRequiredService<IAdoExecuterRepository>();
                            using var _ = repository.CreateDbConnection(connectionString, dbType);
                            await repository.ExecuteAsync("INSERT INTO login_log (Id, Device, Message, Succeed, StatusCode, UserId, Account, Name, RemoteIpAddress, ExecutionTime, CreateTime) VALUES (@Id, @Device, @Message, @Succeed, @StatusCode, @UserId, @Account, @Name, @RemoteIpAddress, @ExecutionTime, @CreateTime)", entities);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "save login_log error");
                        }
                        finally
                        {
                            entities.Clear();
                        }
                    }
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        //save operationlogs
        _ = Task.Factory.StartNew(async () =>
        {
            //using var scope = services.CreateScope();
            //var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<OperationLog>>();
            var channelOperationLogReader = ChannelAccessor<OperationLog>.Instance.Reader;
            var maxAllowInsert = 100;
            var entities = new List<OperationLog>();
            while (await channelOperationLogReader.WaitToReadAsync())
            {
                var currentExistsCount = channelOperationLogReader.Count;
                for (var index = 1; index <= currentExistsCount; index++)
                {
                    var entity = await channelOperationLogReader.ReadAsync();
                    entities.Add(entity);
                    if (index % maxAllowInsert == 0 || index == currentExistsCount)
                    {
                        try
                        {
                            //await repository.AddManyAsync(entities);
                            using var scope = services.CreateScope();
                            var repository = scope.ServiceProvider.GetRequiredService<IAdoExecuterRepository>();
                            using var _ = repository.CreateDbConnection(connectionString, dbType);
                            await repository.ExecuteAsync("INSERT INTO operation_log (Id, ClassName, CreateTime, LogName, LogType, Message, Method, Succeed, UserId, Account, Name, RemoteIpAddress, ExecutionTime) VALUES (@Id, @ClassName, @CreateTime, @LogName, @LogType, @Message, @Method, @Succeed, @UserId, @Account, @Name, @RemoteIpAddress, @ExecutionTime)", entities);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "save operation_log error");
                        }
                        finally
                        {
                            entities.Clear();
                        }
                    }
                }

                if (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        await Task.CompletedTask;
    }
}
