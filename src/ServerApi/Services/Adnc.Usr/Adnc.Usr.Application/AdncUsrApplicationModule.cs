using Autofac;
using Adnc.Application.Shared;
using Microsoft.Extensions.Configuration;

namespace Adnc.Usr.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncUsrApplicationModule : AdncApplicationModule
    {
        public AdncUsrApplicationModule(IConfigurationSection redisSection, IConfigurationSection rabitMqSection) 
            : base(typeof(AdncUsrApplicationModule), redisSection, rabitMqSection) 
        { 
        }

        protected override void Load(ContainerBuilder builder)
        {
            //todo register other types;
            base.Load(builder);
        }
    }
}