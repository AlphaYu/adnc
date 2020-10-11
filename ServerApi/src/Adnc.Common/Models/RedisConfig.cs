using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.Models
{
    /// <summary>
    /// RedisConfig配置
    /// </summary>
    public class RedisConfig
    {
        public int MaxRdSecond { get; set; }
        public bool EnableLogging { get; set; }
        public int LockMs { get; set; }
        public int SleepMs { get; set; }
        public Dbconfig dbconfig { get; set; }
    }

    public class Dbconfig
    {
        public string[] ConnectionStrings { get; set; }
        public bool ReadOnly { get; set; }
    }

}