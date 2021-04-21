using System;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Infr.Common.Helper
{
    public class IdGenerater
    {
        private static int _incr = 0;
        private static readonly object _syncObject = new object();
        public static long DatacenterId { get; private set; } = Environment.GetEnvironmentVariable("DatacenterId").ToLong() ?? 1;
        public static long WorkerId { get; private set; } = Environment.GetEnvironmentVariable("WorkerId").ToLong() ?? 1;

        /// <summary>
        /// 获取唯一Id(不支持多数据中心，每秒并发最大1000)
        /// </summary>
        /// <returns></returns>
        public static long GetNextId()
        {
            var idStr = string.Empty;
            lock (_syncObject)
            {
                _incr = _incr + 1;
                if (_incr > 999)
                    _incr = 0;
                idStr = string.Concat(DateTime.Now.GetTotalSeconds().ToLong(), _incr.ToString().PadLeft(3, '0'));
            }
            return idStr.ToLong().Value;
        }

        /// <summary>
        /// 获取唯一Id(支持高并发，多数据中心)
        /// </summary>
        /// <param name="datacenterId">数据中心Id</param>
        /// <param name="workerId">机器Id</param>
        /// <returns></returns>
        public static long GetNextId(long datacenterId, long workerId)
        {
            return Snowflake.GetInstance(datacenterId, workerId).NextId();
        }
    }
}
