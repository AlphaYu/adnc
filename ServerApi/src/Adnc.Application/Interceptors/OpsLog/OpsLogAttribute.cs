using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Interceptors.OpsLog
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class OpsLogAttribute : Attribute
    {
        public string LogName { get; set; }
    }
}
