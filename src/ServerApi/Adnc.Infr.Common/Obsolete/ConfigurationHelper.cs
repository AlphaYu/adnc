using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Adnc.Common.Helper.Obsolete
{
    public sealed class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        /// <summary>
        ///  初始化Configuration对象，方便类库调用
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]    //多线程同时只能访问一次 
        public static void Initialize(IConfiguration configuration)
        {
            if (_configuration == null)
                _configuration = configuration;
        }

        /// <summary>
        /// 当前引擎
        /// </summary>
        public static IConfiguration Current
        {
            get
            {
                return _configuration;
            }
        }
    }
}
