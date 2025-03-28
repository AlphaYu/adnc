namespace Adnc.Infra.Core.Internal;

internal static class ReflectionDictionary
{
    internal static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertyCache = new();

    internal static readonly ConcurrentDictionary<Type, FieldInfo[]> TypeFieldCache = new();

    internal static readonly ConcurrentDictionary<Type, MethodInfo[]> TypeMethodCache = new();

    internal static readonly ConcurrentDictionary<Type, ConstructorInfo> TypeConstructorCache = new();

    internal static readonly ConcurrentDictionary<Type, Func<object>> TypeEmptyConstructorFuncCache = new();

    internal static readonly ConcurrentDictionary<Type, Func<object[], object>> TypeConstructorFuncCache = new();

    internal static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> PropertyValueGetters = new();

    internal static readonly ConcurrentDictionary<PropertyInfo, Action<object, object>> PropertyValueSetters = new();

    internal static readonly ConcurrentDictionary<Type, object> TypeObejctCache = new();
}

internal static class StrongTypedDictionary<T>
{
    public static readonly ConcurrentDictionary<PropertyInfo, Func<T, object>> PropertyValueGetters = new();

    public static readonly ConcurrentDictionary<PropertyInfo, Action<T, object>> PropertyValueSetters = new();
}