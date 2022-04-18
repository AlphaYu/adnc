using System;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public interface IServiceBuilder
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        string ServiceName { get; set; }

        /// <summary>
        /// Uri方案
        /// </summary>
        string UriScheme { get; set; }

        /// <summary>
        /// 使用哪种策略
        /// </summary>
        ILoadBalancer LoadBalancer { get; set; }

        Task<Uri> BuildAsync(string path);
    }
}