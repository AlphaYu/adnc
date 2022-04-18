using Adnc.Application.Shared;
using Adnc.Infra.Core;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Adnc.Maint.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncMaintApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncMaintApplicationModule(IConfiguration configuration, IServiceInfo serviceInfo)
            : base(typeof(AdncMaintApplicationModule), configuration, serviceInfo) { }

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