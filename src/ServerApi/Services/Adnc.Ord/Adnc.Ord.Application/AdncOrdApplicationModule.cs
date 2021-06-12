using Adnc.Application.Shared;
using Adnc.Infra.Core;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Adnc.Ord.Application
{
    /// <summary>
    /// 订单中心Autofac注册模块
    /// </summary>
    public class AdncOrdApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncOrdApplicationModule(IConfiguration configuration, IServiceInfo serviceInfo)
            : base(typeof(AdncOrdApplicationModule), configuration, serviceInfo)
        {
        }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}