namespace Adnc.Shared.Application.Channels;

public class LogConsumersHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<LogConsumersHostedService> _logger;

    public LogConsumersHostedService(
       ILogger<LogConsumersHostedService> logger,
       IServiceProvider services)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configuration = _services.GetRequiredService<IConfiguration>();
        var dbTypeString = configuration.GetValue<string>(NodeConsts.SysLogDb_DbType);
        var connectionString = configuration.GetValue<string>(NodeConsts.SysLogDb_ConnectionString);
        Checker.ThrowIf(() => dbTypeString.IsNullOrWhiteSpace() || connectionString.IsNullOrWhiteSpace(), "SysLogDb configuration is missing");

        var dbType = dbTypeString.ToUpper().ToEnum<DbTypes>();

        //save loginlogs
        _ = Task.Factory.StartNew(async () =>
        {
            //using var scope = _services.CreateScope();
            //var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<LoginLog>>();
            var channelLoginReader = Accessor<LoginLog>.Instance.Reader;
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
                            using var scope = _services.CreateScope();
                            var repository = scope.ServiceProvider.GetRequiredService<IAdoExecuterRepository>();
                            repository.ChangeOrSetDbConnection(connectionString, dbType);
                            await repository.ExecuteAsync("INSERT INTO login_log (Id, Device, Message, Succeed, StatusCode, UserId, Account, UserName, RemoteIpAddress, CreateTime) VALUES (@Id, @Device, @Message, @Succeed, @StatusCode, @UserId, @Account, @UserName, @RemoteIpAddress, @CreateTime)", entities);
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
            //using var scope = _services.CreateScope();
            //var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<OperationLog>>();
            var channelOperationLogReader = Accessor<OperationLog>.Instance.Reader;
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
                            using var scope = _services.CreateScope();
                            var repository = scope.ServiceProvider.GetRequiredService<IAdoExecuterRepository>();
                            repository.ChangeOrSetDbConnection(connectionString, dbType);
                            await repository.ExecuteAsync("INSERT INTO operation_log (Id, ClassName, CreateTime, LogName, LogType, Message, Method, Succeed, UserId, Account, UserName, RemoteIpAddress) VALUES (@Id, @ClassName, @CreateTime, @LogName, @LogType, @Message, @Method, @Succeed, @UserId, @Account, @UserName, @RemoteIpAddress)", entities);
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