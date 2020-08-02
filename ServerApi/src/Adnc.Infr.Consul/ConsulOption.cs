using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infr.Consul
{
    public class ConsulOption
    {
        /// <summary>
        /// Consul服务注册地址
        /// </summary>
        public string ConsulUrl { get; set; }
        /// <summary>
        /// 当前服务名称，可以多个实例共享
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 当前服务地址[可选，如果Consul帮助类中能获取到，这里可以不配置]
        /// </summary>
        public string ServiceUriHost { get; set; }
        /// <summary>
        /// 当前服务端口号[可选，如果Consul帮助类中能获取到，这里可以不配置]
        /// </summary>
        public int ServiceUriPort { get; set; }
        /// <summary>
        /// 健康检查的地址，当前服务公布出来的一个api接口
        /// </summary>
        public string HealthCheck { get; set; }
    }
}
