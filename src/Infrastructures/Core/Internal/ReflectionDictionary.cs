namespace Adnc.Infra.Core.Internal;

internal static class ReflectionDictionary
{
    internal static ConcurrentDictionary<Type, PropertyInfo[]> TypePropertyCache { get; } = new();

    internal static ConcurrentDictionary<Type, FieldInfo[]> TypeFieldCache { get; } = new();

    internal static ConcurrentDictionary<Type, MethodInfo[]> TypeMethodCache { get; } = new();

    internal static ConcurrentDictionary<Type, ConstructorInfo> TypeConstructorCache { get; } = new();

    internal static ConcurrentDictionary<Type, Func<object>> TypeEmptyConstructorFuncCache { get; } = new();

    internal static ConcurrentDictionary<Type, Func<object[], object>> TypeConstructorFuncCache { get; } = new();

    internal static ConcurrentDictionary<PropertyInfo, Func<object, object>> PropertyValueGetters { get; } = new();

    internal static ConcurrentDictionary<PropertyInfo, Action<object, object>> PropertyValueSetters { get; } = new();

    internal static ConcurrentDictionary<Type, object> TypeObejctCache { get; } = new();
}

internal static class StrongTypedDictionary<T>
{
    public static ConcurrentDictionary<PropertyInfo, Func<T, object>> PropertyValueGetters { get; } = new();

    public static ConcurrentDictionary<PropertyInfo, Action<T, object>> PropertyValueSetters { get; } = new();
}
