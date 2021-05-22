using Adnc.Core.Shared;
using Autofac;

namespace Adnc.Maint.Core
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncMaintCoreModule : AdncCoreModule
    {
        public AdncMaintCoreModule() : base(typeof(AdncMaintCoreModule))
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}