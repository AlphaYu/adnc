using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.Helper.Obsolete
{
    public class StringHelper
    {
        /// <summary>
        /// 首字母转换成大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTheFirstUpper(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            if (str.Length == 1)
                return str.ToUpper();

            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }
    }
}
