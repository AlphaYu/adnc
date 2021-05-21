using Adnc.Application.Shared;
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
        public AdncCusApplicationModule(IConfigurationSection redisSection, IConfigurationSection rabitMqSection)
                    : base(typeof(AdncCusApplicationModule), redisSection, rabitMqSection)
        {
        }

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