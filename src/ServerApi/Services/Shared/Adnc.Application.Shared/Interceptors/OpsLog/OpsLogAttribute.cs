using System;

namespace Adnc.Application.Shared.Interceptors
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class OpsLogAttribute : Attribute
    {
        public string LogName { get; set; }
    }
}
