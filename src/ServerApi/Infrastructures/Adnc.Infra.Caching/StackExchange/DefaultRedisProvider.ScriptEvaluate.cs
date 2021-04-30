using StackExchange.Redis;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : Adnc.Infra.Caching.IRedisProvider
    {
        public RedisResult ScriptEvaluate(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            return _redisDb.ScriptEvaluate(script, keys, values, flags);
        }

        public RedisResult ScriptEvaluate(string script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            var prepared = LuaScript.Prepare(script);
            return _redisDb.ScriptEvaluate(prepared, parameters, flags);
        }
    }
}
