namespace Adnc.Application.Shared.Interceptors.OperateLog;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class OperateLogAttribute : Attribute
{
    public string LogName { get; set; }
}