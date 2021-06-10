using System;

namespace Adnc.Infra.Application.Interceptors
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// 需要把事务共享给CAP
        /// </summary>
        public bool SharedToCap { get; set; }
    }
}