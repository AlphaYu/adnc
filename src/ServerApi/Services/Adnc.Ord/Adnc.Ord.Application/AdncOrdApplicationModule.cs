using Adnc.Application.Shared;
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
        public AdncOrdApplicationModule(IConfigurationSection redisSection, IConfigurationSection rabitMqSection)
            : base(typeof(AdncOrdApplicationModule), redisSection, rabitMqSection)
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