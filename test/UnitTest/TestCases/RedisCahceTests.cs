using Adnc.Infra.Redis;
using Adnc.Infra.Redis.Caching;

namespace Adnc.UnitTest.TestCases;

public class RedisCahceTests : IClassFixture<RedisCacheFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly ICacheProvider _cacheProvider;
    private readonly IRedisProvider _redisProvider;
    private readonly IDistributedLocker _distributedLocker;
    private readonly RedisCacheFixture _fixture;

    public RedisCahceTests(RedisCacheFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _cacheProvider = _fixture.Container.GetRequiredService<ICacheProvider>();
        _redisProvider = _fixture.Container.GetRequiredService<IRedisProvider>();
        _distributedLocker = _fixture.Container.GetRequiredService<IDistributedLocker>();
    }

    [Fact]
    public void TestString()
    {
        var key = nameof(TestString).ToLower();
        var value = DateTime.Now.Ticks;
        var result = _redisProvider.StringSet(key, value.ToString(), TimeSpan.FromSeconds(10));
        Assert.True(result);
        var actual = _redisProvider.StringGet(key);
        Assert.Equal(value.ToString(), actual);
    }

    /// <summary>
    /// 测试lua脚本
    /// </summary>
    [Fact]
    public async Task TestScriptEvaluateStoreSet()
    {
        var cacheKey = nameof(TestScriptEvaluateStoreSet).ToLower();

        var set = new Dictionary<long, double>();
        for (long index = 0; index < 64; index++)
        {
            set.Add(index, DateTime.Now.GetTotalMicroseconds());
        }

        _redisProvider.ZAdd(cacheKey, set);

        var scirpt = @"local workerids = redis.call('ZRANGE', @key, @start,@stop)
                                    redis.call('ZADD',@key,@score,workerids[1])
                                    return workerids[1]";
        var parameters = new { key = cacheKey, start = 0, stop = 0, score = DateTime.Now.GetTotalMicroseconds() };

        var luaResult = (byte[])await _redisProvider.ScriptEvaluateAsync(scirpt, parameters);
        var workerId = _cacheProvider.Serializer.Deserialize<long>(luaResult);
        _output.WriteLine(workerId.ToString());
        Assert.True(workerId >= 0);
    }

    /// <summary>
    /// 测试lua脚本
    /// </summary>
    [Fact]
    public void TestScriptEvaluateString()
    {
        var milliseconds = 1000 * 60 * 1000;
        var cacheKey = nameof(TestScriptEvaluateString).ToLower();
        var value = "abc";

        _redisProvider.StringSet(cacheKey, value, TimeSpan.FromSeconds(1000));

        var script = @"local val = redis.call('GET', @key)
                                    if val==@value then
                                        redis.call('PEXPIRE', @key, @milliseconds)
                                        return 1
                                    end
                                    return 0";
        var parameters = new { key = cacheKey, value, milliseconds };
        var luaResult = _redisProvider.ScriptEvaluateAsync(script, parameters).GetAwaiter().GetResult();
        var result = (int)luaResult;

        _output.WriteLine(result.ToString());
        Assert.True(result == 1);
    }

    /// <summary>
    /// 测试分布式锁
    /// </summary>
    [Fact]
    public async Task TestDistributedLocker()
    {
        //autodelay = false
        var cacheKey = nameof(TestDistributedLocker).ToLower() + "-" + new Random().Next(10000, 20000).ToString();

        var flagtrue = await _distributedLocker.LockAsync(cacheKey, 20, false);
        Assert.True(flagtrue.Success);

        var flagfalse = await _distributedLocker.LockAsync(cacheKey, 20, false);
        Assert.False(flagfalse.Success);

        var unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, "111");
        Assert.False(unLockResult);

        flagfalse = await _distributedLocker.LockAsync(cacheKey, 20, false);
        Assert.False(flagfalse.Success);

        unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, flagtrue.LockValue);
        Assert.True(unLockResult);

        //autodelay = true
        cacheKey = nameof(TestDistributedLocker).ToLower() + "-" + new Random().Next(20001, 30000).ToString();
        flagtrue = await _distributedLocker.LockAsync(cacheKey, 3, true);
        Assert.True(flagtrue.Success);

        await Task.Delay(1000 * 10);

        flagfalse = await _distributedLocker.LockAsync(cacheKey, 20);
        Assert.False(flagfalse.Success);

        unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, "111");
        Assert.False(unLockResult);

        unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, flagtrue.LockValue);
        Assert.True(unLockResult);
    }

    [Fact]
    public void TestRemoveAll()
    {
        _cacheProvider.CacheOptions.Value.PenetrationSetting.Disable = true;

        var key01 = "TestRemoveAll01".ToLower();
        var key02 = "TestRemoveAll02".ToLower();
        var value = DateTime.Now.Ticks;

        _cacheProvider.Set(key01, value, TimeSpan.FromSeconds(100));
        _cacheProvider.Set(key02, value, TimeSpan.FromSeconds(100));

        var actual = _cacheProvider.Get<long>(key02);
        Assert.Equal(value, actual.Value);

        var keys = new List<string>() { key01, key02 };
        _cacheProvider.RemoveAll(keys);
        var cacheValue = _cacheProvider.Get<long>(key01);
        Assert.False(cacheValue.HasValue);
    }

    [Fact]
    public void TestIncr()
    {
        var cacheKey = nameof(TestIncr).ToLower();
        var cacheValue = _redisProvider.IncrBy(cacheKey, 1);
        _output.WriteLine(cacheValue.ToString());
        Assert.True(cacheValue > 0);
    }

    [Fact]
    public void TestSortSet()
    {
        var cacheKey = nameof(TestSortSet).ToLower();
        _redisProvider.ZAdd(cacheKey, new Dictionary<long, double> { { 1, DateTime.Now.GetTotalMilliseconds() } });
        dynamic returnReulst = _redisProvider.ZRange<long>(cacheKey, 0, 0);
        _output.WriteLine(returnReulst[0].ToString());
        Assert.NotNull(returnReulst);

        // 返回有序集合中指定成员的索引
        returnReulst = _redisProvider.ZRank<long>(cacheKey, 1);
        _output.WriteLine(returnReulst.ToString());
        Assert.True(returnReulst >= 0);
    }

    [Fact]
    public async Task TestKeyExpire()
    {
        _cacheProvider.CacheOptions.Value.PenetrationSetting.Disable = true;

        var cacheKey1 = nameof(TestKeyExpire).ToLower() + ":" + DateTime.Now.Ticks;
        var cacheKey2 = nameof(TestKeyExpire).ToLower() + ":" + DateTime.Now.Ticks;
        var cacheKey3 = nameof(TestKeyExpire).ToLower() + ":" + DateTime.Now.Ticks;
        var cacheKey4 = "cacheKey4";

        await _cacheProvider.SetAsync(cacheKey1, nameof(cacheKey1), TimeSpan.FromSeconds(1000));
        await _cacheProvider.SetAsync(cacheKey2, nameof(cacheKey2), TimeSpan.FromSeconds(1000));
        await _cacheProvider.SetAsync(cacheKey3, nameof(cacheKey3), TimeSpan.FromSeconds(1000));

        var seconds = await _redisProvider.TTLAsync(cacheKey1);
        Assert.True(seconds > 990);

        await _cacheProvider.KeyExpireAsync(new string[] { cacheKey1, cacheKey2, cacheKey3, cacheKey4 }, 100);
        var seconds1 = await _redisProvider.TTLAsync(cacheKey1);
        var seconds2 = await _redisProvider.TTLAsync(cacheKey2);
        var seconds3 = await _redisProvider.TTLAsync(cacheKey3);

        Assert.True(seconds1 <= 100 && seconds1 > 90);
        Assert.True(seconds2 <= 100 && seconds1 > 90);
        Assert.True(seconds3 <= 100 && seconds1 > 90);

        var exists = await _cacheProvider.ExistsAsync(cacheKey4);
        Assert.False(exists);

        var value = await _cacheProvider.GetAsync<string>(cacheKey3);
        Assert.Equal("cacheKey3", value.Value);
    }

    /// <summary>
    /// 测试布隆过滤器
    /// </summary>
    [Fact]
    public async Task TestBloomFilter()
    {
        var cacheKey = nameof(TestBloomFilter).ToLower();

        await _redisProvider.KeyDelAsync(cacheKey);

        await _redisProvider.BfReserveAsync(cacheKey, 0.001, 10000000);

        var initValues = new List<string>();
        for (int index = 0; index < 100000; index++) initValues.Add($"adnc{index}");
        await _redisProvider.BfAddAsync(cacheKey, initValues);

        var trueResult = await _redisProvider.BfExistsAsync(cacheKey, "adnc100");
        Assert.True(trueResult);

        var falseResult = await _redisProvider.BfExistsAsync(cacheKey, "adnc");
        Assert.False(falseResult);

        var values = new List<string>() { "adnc88888", "adnc78888", "adnc68888", "adnc58888" };
        var results = await _redisProvider.BfExistsAsync(cacheKey, values);
        var trueLength = results.Where(x => x == true).Count();
        Assert.Equal(values.Count, trueLength);

        values = new List<string>() { "danc888889", "danc888888", "danc8888888", "danc8888889" };
        results = await _redisProvider.BfExistsAsync(cacheKey, values);
        var falseLength = results.Where(x => x == false).Count();
        Assert.Equal(values.Count, falseLength);
    }
}
