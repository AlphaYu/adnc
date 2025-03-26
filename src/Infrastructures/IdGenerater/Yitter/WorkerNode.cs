namespace Adnc.Infra.IdGenerater.Yitter;

public sealed class WorkerNode(ILogger<WorkerNode> logger, IRedisProvider redisProvider, IDistributedLocker distributedLocker, string name)
{
    private readonly ILogger<WorkerNode> _logger = logger;
    private readonly IRedisProvider _redisProvider = redisProvider;
    private readonly IDistributedLocker _distributedLocker = distributedLocker;
    private readonly string _name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException("workernode.servicename is empty or null") : name;
    private readonly string _redisKey = $"adnc:{name}:workids";

    internal async Task InitWorkerNodesAsync()
    {
        if (!_redisProvider.KeyExists(_redisKey))
        {
            _logger.LogInformation("Starting InitWorkerNodes:{0}", _redisKey);

            var flag = await _distributedLocker.LockAsync(_redisKey);

            if (!flag.Success)
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
                await _distributedLocker.SafedUnLockAsync(_redisKey, flag.LockValue);
            }

            _logger.LogInformation("Finlished InitWorkerNodes:{0}:{1}", _redisKey, count);
        }
        else
        {
            _logger.LogInformation("Exists WorkerNodes:{0}", _redisKey);
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

        _logger.LogInformation("Get WorkerNodes:{0}", workerId);

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
        _logger.LogInformation("Refresh WorkerNodes:{0}:{1}", workerId, score);
    }

    public string GetWorkerNodeName() => _name;
}