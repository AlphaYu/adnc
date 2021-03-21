using Autofac;
using Adnc.Application.Shared;

namespace Adnc.Maint.Application
{

    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncMaintApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncMaintApplicationModule() : base(typeof(AdncMaintApplicationModule)) { }

        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //todo register other types;
            base.Load(builder);
        }
    }
}