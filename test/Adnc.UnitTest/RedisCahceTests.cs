using System;
using System.Collections.Generic;
using Autofac;
using Xunit;
using Xunit.Abstractions;
using Adnc.Infra.Caching;
using Adnc.UnitTest.Fixtures;
using Adnc.Infra.Common.Extensions;

namespace Adnc.UnitTest.Cache
{
    public class RedisCahceTests : IClassFixture<RedisCacheFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly ICacheProvider _cache;
        private readonly IRedisProvider _redisProvider;
        private readonly IDistributedLocker _distributedLocker;
        private RedisCacheFixture _fixture;

        public RedisCahceTests(RedisCacheFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _cache = _fixture.Container.Resolve<ICacheProvider>();
            _redisProvider = _fixture.Container.Resolve<IRedisProvider>();
            _distributedLocker = _fixture.Container.Resolve<IDistributedLocker>();
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

        [Fact]
        public async void TestScriptEvaluateStoreSet()
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

            var luaResult = (byte[]) await _redisProvider.ScriptEvaluateAsync(scirpt, parameters);
            var workerId = _cache.Serializer.Deserialize<long>(luaResult);
            _output.WriteLine(workerId.ToString());
            Assert.True(workerId >= 0);
        }

        [Fact]
        public async void TestDistributedLocker()
        {
            var cacheValue = new Random().Next(1000, 9999).ToString();
            var cacheKey = nameof(TestDistributedLocker).ToLower()+":"+ cacheValue;

            var flagtrue = await _distributedLocker.LockAsync(cacheKey, cacheValue, TimeSpan.FromSeconds(20));
            Assert.True(flagtrue);

            var flagfalse = await _distributedLocker.LockAsync(cacheKey, cacheValue,TimeSpan.FromSeconds(20));
            Assert.False(flagfalse);

            var unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, "111");
            Assert.False(unLockResult);

            flagfalse = await _distributedLocker.LockAsync(cacheKey, cacheValue, TimeSpan.FromSeconds(20));
            Assert.False(flagfalse);

            unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, cacheValue);
            Assert.True(unLockResult);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var key01 = "TestRemoveAll01".ToLower();
            var key02 = "TestRemoveAll02".ToLower();
            var value = DateTime.Now.Ticks;

            _cache.Set(key01, value, TimeSpan.FromSeconds(100));
            _cache.Set(key02, value, TimeSpan.FromSeconds(100));

            var actual = _cache.Get<long>(key02);
            Assert.Equal(value, actual.Value);

            var keys = new List<string>() { key01, key02 };
            _cache.RemoveAll(keys);
            var cacheValue = _cache.Get<long>(key01);
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
    }
}
