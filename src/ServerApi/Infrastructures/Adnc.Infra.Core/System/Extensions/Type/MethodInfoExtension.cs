using System.Text.RegularExpressions;

namespace System.Reflection;

public static class MethodInfoExtension
{
    public static bool IsAsyncMethod(this MethodInfo methodInfo)
    {
        return (
            methodInfo.ReturnType == typeof(Task)
            || (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        );
    }
   
    public static string GetMethodName(this MethodBase methodInfo)
    {
        var originalMethodName = methodInfo.DeclaringType?.Name ?? string.Empty;
        if (string.IsNullOrWhiteSpace(originalMethodName))
            return string.Empty;

        var match = Regex.Match(originalMethodName, "<(.*)>");
        return match.Success ? match.Groups[1].Value : originalMethodName;
    }
}