using StackExchange.Redis;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Default redis caching provider.
    /// </summary>
    public partial class DefaultRedisProvider : Adnc.Infra.Caching.IRedisProvider
    {
        public dynamic ScriptEvaluate(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            return _redisDb.ScriptEvaluate(script, keys, values, flags);
        }

        public dynamic ScriptEvaluate(string script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            var prepared = LuaScript.Prepare(script);
            var result = _redisDb.ScriptEvaluate(prepared, parameters, flags);
            return result;
        }
    }
}
