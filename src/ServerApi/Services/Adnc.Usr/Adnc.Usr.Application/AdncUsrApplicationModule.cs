using Autofac;
using Adnc.Application.Shared;

namespace Adnc.Usr.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncUsrApplicationModule : AdncApplicationModule
    {
        public AdncUsrApplicationModule(): base(typeof(AdncUsrApplicationModule)) { }

        protected override void Load(ContainerBuilder builder)
        {
            //todo register other types;
            base.Load(builder);
        }
    }
}