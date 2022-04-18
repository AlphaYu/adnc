using Adnc.Application.Shared;
using Adnc.Infra.Core;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Adnc.Usr.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncUsrApplicationModule : AdncApplicationModule
    {
        public AdncUsrApplicationModule(IConfiguration configuration, IServiceInfo serviceInfo)
            : base(typeof(AdncUsrApplicationModule), configuration, serviceInfo)
        {
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}