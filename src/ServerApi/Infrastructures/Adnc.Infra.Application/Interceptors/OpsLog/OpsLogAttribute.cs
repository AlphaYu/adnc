using System;

namespace Adnc.Infra.Application.Interceptors
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class OpsLogAttribute : Attribute
    {
        public string LogName { get; set; }
    }
}