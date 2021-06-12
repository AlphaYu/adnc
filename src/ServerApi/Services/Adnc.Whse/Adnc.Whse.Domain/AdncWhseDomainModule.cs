using Adnc.Domain.Shared;
using Autofac;

namespace Adnc.Whse.Domain
{
    public class AdncWhseDomainModule : AdncDomainModule
    {
        public AdncWhseDomainModule() : base(typeof(AdncWhseDomainModule))
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

        /// <summary>
        /// Autofac注册,该方法供UnitTest工程使用
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        public static void Register(ContainerBuilder builder)
        {
            new AdncWhseDomainModule().Load(builder);
        }
    }
}