using Autofac;
using Adnc.Application.Shared;

namespace Adnc.Cus.Application
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public sealed class AdncCusApplicationModule : AdncApplicationModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AdncCusApplicationModule() : base(typeof(AdncCusApplicationModule)) { }

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