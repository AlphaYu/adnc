using Adnc.Infra.Repository;
using Autofac;

namespace Adnc.Cus.Repository
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncCusRepositoryModule : AdncRepositoryModule
    {
        public AdncCusRepositoryModule() : base(typeof(AdncCusRepositoryModule))
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
            new AdncCusRepositoryModule().Load(builder);
        }
    }
}