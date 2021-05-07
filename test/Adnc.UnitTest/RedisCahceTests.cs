using System;
using System.Collections.Generic;
using Autofac;
using Xunit;
using Xunit.Abstractions;
using Adnc.Infra.Caching;
using Adnc.UnitTest.Fixtures;
using Adnc.Infra.Common.Extensions;
using System.Threading.Tasks;

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

        /// <summary>
        /// 测试lua脚本
        /// </summary>
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

        /// <summary>
        /// 测试lua脚本
        /// </summary>
        [Fact]
        public void TestScriptEvaluateString()
        {
            var milliseconds = 1000*60*1000;
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
        public async void TestDistributedLocker()
        {
            //autodelay = false
            var cacheKey = nameof(TestDistributedLocker).ToLower()+"-"+ new Random().Next(10000, 20000).ToString();

            var flagtrue = await _distributedLocker.LockAsync(cacheKey, 20, false);
            Assert.True(flagtrue.Success);

            var flagfalse = await _distributedLocker.LockAsync(cacheKey,20, false);
            Assert.False(flagfalse.Success);

            var unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, "111");
            Assert.False(unLockResult);

            flagfalse = await _distributedLocker.LockAsync(cacheKey,20, false);
            Assert.False(flagfalse.Success);

            unLockResult = await _distributedLocker.SafedUnLockAsync(cacheKey, flagtrue.LockValue);
            Assert.True(unLockResult);

            //autodelay = true
            cacheKey = nameof(TestDistributedLocker).ToLower() + "-" + new Random().Next(20001, 30000).ToString();
            flagtrue = await _distributedLocker.LockAsync(cacheKey, 3);
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

        /// <summary>
        /// 测试穿透
        /// </summary>
        //[Fact]
        //public void TestPenetrate()
        //{
        //    var cacheKey = nameof(TestPenetrate).ToLower();
        //    var cacheValue = _cache.Get<object>(cacheKey, () =>
        //     {
        //         Assert.True(false);
        //         return null;
        //     }, TimeSpan.FromSeconds(5));

        //    cacheValue = _cache.Get<object>(cacheKey, () =>
        //    {
        //        Assert.True(false);
        //        return null;
        //    }, TimeSpan.FromSeconds(5));


        //}
    }
}
