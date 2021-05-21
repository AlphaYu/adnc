using Adnc.Application.Shared;
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
        public AdncMaintApplicationModule(IConfigurationSection redisSection, IConfigurationSection rabitMqSection)
            : base(typeof(AdncMaintApplicationModule), redisSection, rabitMqSection) { }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //todo register other types;
            base.Load(builder);
        }
    }
}