using Autofac;
using Adnc.Core.Shared;

namespace Adnc.Usr.Core
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncUsrCoreModule : AdncCoreModule
    {
        public AdncUsrCoreModule() : base(typeof(AdncUsrCoreModule)) { }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}