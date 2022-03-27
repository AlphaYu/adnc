namespace Adnc.Application.Contracts.Shared.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class OperateLogAttribute : Attribute
{
    public string LogName { get; set; }
}