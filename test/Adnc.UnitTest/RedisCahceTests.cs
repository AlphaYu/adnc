using System;
using Autofac;
using Adnc.UnitTest.Fixtures;
using Xunit;
using Xunit.Abstractions;
using Adnc.Infra.Caching;
using System.Collections.Generic;

namespace Adnc.UnitTest.Cache
{
    public class RedisCahceTests : IClassFixture<RedisCacheFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly IRedisDistributedCache _cache;
        private readonly IRedisProvider _redisProvider;
        private RedisCacheFixture _fixture;

        public RedisCahceTests(RedisCacheFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _cache = _fixture.Container.Resolve<IRedisDistributedCache>();
            _redisProvider = _fixture.Container.Resolve<IRedisProvider>();
        }

        [Fact]
        public void TestString()
        {
            var key = "TestString";
            var value = DateTime.Now.Ticks;
            var result = _redisProvider.StringSet(key, value.ToString(), TimeSpan.FromSeconds(10));
            Assert.True(result);
            var actual = _redisProvider.StringGet(key);
            Assert.Equal(value.ToString(), actual);
        }

        [Fact]
        public void TestScriptEvaluate()
        {
            var key = "TestScriptEvaluate";
            var value = DateTime.Now.Ticks;
            var scirpt = @"redis.call('set', @key, @value,'ex',@ex)";
            var parameters = new { key = key, value = value, ex = 10 };
           _redisProvider.ScriptEvaluate(scirpt, parameters);
            var actual = _redisProvider.StringGet(key);
            Assert.Equal(value.ToString(), actual);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var key01 = "TestRemoveAll01";
            var key02 = "TestRemoveAll02";
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
    }
}
