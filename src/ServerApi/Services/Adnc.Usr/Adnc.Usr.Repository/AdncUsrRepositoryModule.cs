using Adnc.Infra.Repository;
using Autofac;

namespace Adnc.Usr.Repository
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncUsrRepositoryModule : AdncRepositoryModule
    {
        public AdncUsrRepositoryModule() : base(typeof(AdncUsrRepositoryModule))
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