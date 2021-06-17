using System;

namespace Adnc.Application.Shared.Interceptors
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class OperateLogAttribute : Attribute
    {
        public string LogName { get; set; }
    }
}