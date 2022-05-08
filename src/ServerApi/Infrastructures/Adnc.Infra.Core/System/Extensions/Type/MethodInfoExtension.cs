namespace System.Reflection;

public static class MethodInfoExtension
{
    public static bool IsAsyncMethod(this MethodInfo @this)
    {
        return (
            @this.ReturnType == typeof(Task)
            || (@this.ReturnType.IsGenericType && @this.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        );
    }
}