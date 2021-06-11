using Adnc.Infra.Repository;
using Autofac;

namespace Adnc.Maint.Repository
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncMaintRepositoryModule : AdncRepositoryModule
    {
        public AdncMaintRepositoryModule() : base(typeof(AdncMaintRepositoryModule))
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