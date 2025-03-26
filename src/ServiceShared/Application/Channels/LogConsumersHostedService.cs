namespace Adnc.Shared.Application.Channels;

public class LogConsumersHostedService(ILogger<LogConsumersHostedService> logger, IServiceProvider services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var dbTypeString = configuration.GetValue<string>(NodeConsts.SysLogDb_DbType);
        var connectionString = configuration.GetValue<string>(NodeConsts.SysLogDb_ConnectionString);

        if (string.IsNullOrWhiteSpace(dbTypeString) || string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException("SysLogDb configuration is missing");
        }

        var dbType = dbTypeString.ToUpper().ToEnum<DbTypes>();

        //save loginlogs
        _ = Task.Factory.StartNew(async () =>
        {
            //using var scope = services.CreateScope();
            //var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LoginLog>>();
            var channelLoginReader = LogAccessor<LoginLog>.Instance.Reader;
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
                            //await repository.AddManyAsync(entities);
                            using var scope = services.CreateScope();
                            var repository = scope.ServiceProvider.GetRequiredService<IAdoExecuterRepository>();
                            using var _ = repository.ChangeOrSetDbConnection(connectionString, dbType);
                            await repository.ExecuteAsync("INSERT INTO login_log (Id, Device, Message, Succeed, StatusCode, UserId, Account, Name, RemoteIpAddress, ExecutionTime, CreateTime) VALUES (@Id, @Device, @Message, @Succeed, @StatusCode, @UserId, @Account, @Name, @RemoteIpAddress, @ExecutionTime, @CreateTime)", entities);
                        }
                        catch (Exception ex)
                        {
                            var message = $"{nameof(ExecuteAsync)}:{nameof(channelLoginReader)}";
                            logger.LogError(ex, message);
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
            var channelOperationLogReader = LogAccessor<OperationLog>.Instance.Reader;
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
                            //await repository.AddManyAsync(entities);
                            using var scope = services.CreateScope();
                            var repository = scope.ServiceProvider.GetRequiredService<IAdoExecuterRepository>();
                            using var _ = repository.ChangeOrSetDbConnection(connectionString, dbType);
                            await repository.ExecuteAsync("INSERT INTO operation_log (Id, ClassName, CreateTime, LogName, LogType, Message, Method, Succeed, UserId, Account, Name, RemoteIpAddress, ExecutionTime) VALUES (@Id, @ClassName, @CreateTime, @LogName, @LogType, @Message, @Method, @Succeed, @UserId, @Account, @Name, @RemoteIpAddress, @ExecutionTime)", entities);
                        }
                        catch (Exception ex)
                        {
                            var message = $"{nameof(ExecuteAsync)}:{nameof(channelOperationLogReader)}";
                            logger.LogError(ex, message);
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