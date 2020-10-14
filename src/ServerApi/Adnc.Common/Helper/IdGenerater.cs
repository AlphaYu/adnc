using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Common;
using Adnc.Common.Extensions;

namespace Adnc.Common.Helper
{
    public class IdGeneraterHelper
    {
        /// <summary>
        /// 临时方法，以后会调整
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Obsolete]
        public static long GetNextId(IdGeneraterKey key, int incr = -1)
        {
            var idStr = string.Concat((int)key, DateTime.Now.GetTimestamp(), DateTime.Now.Millisecond.ToString().PadLeft(3, '0'));
            return idStr.ToLong().Value + incr;
        }
    }

    public enum IdGeneraterKey
    {
        CFG = 1,
        USER = 2,
        DICT = 3,
        Task = 4,
        DEPT = 5,
        MENU = 6,
        ROLE = 7,
        PEMS=8,
    }

}
