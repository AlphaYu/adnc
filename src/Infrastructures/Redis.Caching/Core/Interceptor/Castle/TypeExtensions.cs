namespace Adnc.Infra.Redis.Caching.Core.Interceptor.Castle;

public static class TypeExtensions
{
    private static readonly ConcurrentDictionary<TypeInfo, bool> _isTaskOfTCache = new();

    public static bool IsTaskWithResult(this TypeInfo typeInfo)
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        return _isTaskOfTCache.GetOrAdd(typeInfo, Info => Info.IsGenericType && typeof(Task).GetTypeInfo().IsAssignableFrom(Info));
    }

    public static bool IsTask(this TypeInfo typeInfo)
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        return typeInfo.AsType() == typeof(Task);
    }
}
