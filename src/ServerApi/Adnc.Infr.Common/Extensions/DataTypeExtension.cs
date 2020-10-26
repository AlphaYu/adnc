using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infr.Common.Extensions
{
    public static class DataTypeExtension
    {
        /// <summary>
        /// timestamp=>datetime(东八区)
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToLocalTime(this long timestamp)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dto.ToLocalTime().DateTime;
        }


        /// <summary>
        /// datetime=>timestamp(东八区)
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static long GetTimestamp(this DateTime currenDt)
        {
            return (currenDt.ToUniversalTime().Ticks - 621355968000000000)/10000000;
        }

        /// <summary>
        /// string=>long
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static long? ToLong(this string txt)
        {
            long result;
            bool status = long.TryParse(txt, out result);

            if (status)
                return result;
            else
                return null;
        }
    }
}
