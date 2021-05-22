using Adnc.Core.Shared;
using Autofac;

namespace Adnc.Cus.Core
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncCusCoreModule : AdncCoreModule
    {
        public AdncCusCoreModule() : base(typeof(AdncCusCoreModule))
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

        /// <summary>
        /// Autofac注册,该方法供UnitTest工程使用
        /// </summary>
        /// <param name="builder"></param>
        public static void Register(ContainerBuilder builder)
        {
            new AdncCusCoreModule().Load(builder);
        }
    }
}