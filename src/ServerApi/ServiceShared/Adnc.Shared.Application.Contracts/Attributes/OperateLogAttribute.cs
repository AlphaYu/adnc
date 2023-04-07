namespace Adnc.Shared.Application.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class OperateLogAttribute : Attribute
{
    public string LogName { get; set; } = string.Empty;
}