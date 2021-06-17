using Adnc.Application.Shared;
using Adnc.Infra.Core;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Adnc.Whse.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncWhseApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncWhseApplicationModule(IConfiguration configuration, IServiceInfo serviceInfo)
                    : base(typeof(AdncWhseApplicationModule), configuration, serviceInfo)
        {
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}