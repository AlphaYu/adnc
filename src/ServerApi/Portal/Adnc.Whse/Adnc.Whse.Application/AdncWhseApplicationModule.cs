using Autofac;
using Adnc.Application.Shared;

namespace Adnc.Whse.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncWhseApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncWhseApplicationModule() : base(typeof(AdncWhseApplicationModule)) { }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}