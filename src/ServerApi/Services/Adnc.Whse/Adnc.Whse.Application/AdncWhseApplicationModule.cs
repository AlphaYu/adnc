using Autofac;
using Adnc.Application.Shared;
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
        public AdncWhseApplicationModule(IConfigurationSection redisSection, IConfigurationSection rabitMqSection)
                    : base(typeof(AdncWhseApplicationModule), redisSection, rabitMqSection)
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