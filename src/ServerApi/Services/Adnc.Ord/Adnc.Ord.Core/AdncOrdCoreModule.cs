using Autofac;
using Adnc.Core.Shared;

namespace Adnc.Ord.Core
{
    public class AdncOrdCoreModule : AdncCoreModule
    {
        public AdncOrdCoreModule() : base(typeof(AdncOrdCoreModule)) { }

        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
