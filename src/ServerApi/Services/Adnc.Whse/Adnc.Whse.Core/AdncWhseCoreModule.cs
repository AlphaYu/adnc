using Adnc.Core.Shared;
using Autofac;

namespace Adnc.Whse.Core
{
    public class AdncWhseCoreModule : AdncCoreModule
    {
        public AdncWhseCoreModule() : base(typeof(AdncWhseCoreModule))
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
            new AdncWhseCoreModule().Load(builder);
        }
    }
}