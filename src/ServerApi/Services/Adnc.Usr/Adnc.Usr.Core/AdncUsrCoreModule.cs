using Adnc.Core.Shared;
using Autofac;

namespace Adnc.Usr.Core
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncUsrCoreModule : AdncCoreModule
    {
        public AdncUsrCoreModule() : base(typeof(AdncUsrCoreModule))
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