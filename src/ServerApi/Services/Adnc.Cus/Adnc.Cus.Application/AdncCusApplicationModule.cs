using Adnc.Application.Shared;
using Adnc.Infra.Core;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Adnc.Cus.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncCusApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncCusApplicationModule(IConfiguration configuration, IServiceInfo serviceInfo)
                    : base(typeof(AdncCusApplicationModule), configuration, serviceInfo)
        {
        }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}