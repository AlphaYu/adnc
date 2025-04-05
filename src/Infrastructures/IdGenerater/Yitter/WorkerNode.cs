namespace Adnc.Infra.IdGenerater.Yitter;

public sealed class WorkerNode(ILogger<WorkerNode> logger, IRedisProvider redisProvider, IDistributedLocker distributedLocker, string name)
{
    private readonly IDistributedLocker _distributedLocker = distributedLocker;
    private readonly ILogger<WorkerNode> _logger = logger;
    private readonly string _name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException("workernode.servicename is empty or null") : name;
    private readonly string _redisKey = $"adnc:{name}:workids";
    private readonly IRedisProvider _redisProvider = redisProvider;

    internal async Task InitWorkerNodesAsync()
    {
        if (!_redisProvider.KeyExists(_redisKey))
        {
            _logger.LogInformation("Starting InitWorkerNodes:{_redisKey}", _redisKey);

            var (success, lockValue) = await _distributedLocker.LockAsync(_redisKey);

            if (!success)
            {
                await Task.Delay(300);
                await InitWorkerNodesAsync();
            }

            long count = 0;
            try
            {
                var set = new Dictionary<long, double>();
                for (long index = 0; index <= IdGenerater.MaxWorkerId; index++)
                {
                    set.Add(index, DateTime.Now.GetTotalMilliseconds());
                }
                count = await _redisProvider.ZAddAsync(_redisKey, set);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await _distributedLocker.SafedUnLockAsync(_redisKey, lockValue);
            }

            _logger.LogInformation("Finlished InitWorkerNodes:{_redisKey}:{count}", _redisKey, count);
        }
        else
        {
            _logger.LogInformation("Exists WorkerNodes:{_redisKey}", _redisKey);
        }
    }

    internal async Task<long> GetWorkerIdAsync()
    {
        var scirpt = @"local workerids = redis.call('ZRANGE', @key, @start,@stop)
                                    redis.call('ZADD',@key,@score,workerids[1])
                                    return workerids[1]";

        var parameters = new { key = _redisKey, start = 0, stop = 0, score = DateTime.Now.GetTotalMilliseconds() };
        var luaResult = (byte[])await _redisProvider.ScriptEvaluateAsync(scirpt, parameters);
        var workerId = _redisProvider.Serializer.Deserialize<long>(luaResult);

        _logger.LogInformation("Get WorkerNodes:{workerId}", workerId);

        return workerId;
    }

    internal async Task RefreshWorkerIdScoreAsync(long workerId, double? workerIdScore = null)
    {
        if (workerId < 0 || workerId > IdGenerater.MaxWorkerId)
        {
            throw new InvalidDataException(string.Format("worker Id can't be greater than {0} or less than 0", IdGenerater.MaxWorkerId));
        }

        var score = workerIdScore == null ? DateTime.Now.GetTotalMilliseconds() : workerIdScore.Value;
        await _redisProvider.ZAddAsync(_redisKey, new Dictionary<long, double> { { workerId, score } });
        _logger.LogInformation("Refresh WorkerNodes:{workerId}:{score}", workerId, score);
    }

    public string GetWorkerNodeName() => _name;
}
