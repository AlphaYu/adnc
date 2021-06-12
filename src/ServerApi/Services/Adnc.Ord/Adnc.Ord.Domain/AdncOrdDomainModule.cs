using Adnc.Domain.Shared;
using Autofac;

namespace Adnc.Ord.Domain
{
    public class AdncOrdDomainModule : AdncDomainModule
    {
        public AdncOrdDomainModule() : base(typeof(AdncOrdDomainModule))
        {
        }

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