namespace Adnc.Infra.Core.Internal
{
    internal static class ReflectionDictionary
    {
        internal static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        internal static readonly ConcurrentDictionary<Type, FieldInfo[]> TypeFieldCache = new ConcurrentDictionary<Type, FieldInfo[]>();

        internal static readonly ConcurrentDictionary<Type, MethodInfo[]> TypeMethodCache = new ConcurrentDictionary<Type, MethodInfo[]>();

        internal static readonly ConcurrentDictionary<Type, ConstructorInfo> TypeConstructorCache = new ConcurrentDictionary<Type, ConstructorInfo>();

        internal static readonly ConcurrentDictionary<Type, Func<object>> TypeEmptyConstructorFuncCache = new ConcurrentDictionary<Type, Func<object>>();

        internal static readonly ConcurrentDictionary<Type, Func<object[], object>> TypeConstructorFuncCache = new ConcurrentDictionary<Type, Func<object[], object>>();

        internal static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> PropertyValueGetters = new ConcurrentDictionary<PropertyInfo, Func<object, object>>();

        internal static readonly ConcurrentDictionary<PropertyInfo, Action<object, object>> PropertyValueSetters = new ConcurrentDictionary<PropertyInfo, Action<object, object>>();

        internal static readonly ConcurrentDictionary<Type, object> TypeObejctCache = new ConcurrentDictionary<Type, object>();
    }

    internal static class StrongTypedDictionary<T>
    {
        public static readonly ConcurrentDictionary<PropertyInfo, Func<T, object>> PropertyValueGetters = new ConcurrentDictionary<PropertyInfo, Func<T, object>>();

        public static readonly ConcurrentDictionary<PropertyInfo, Action<T, object>> PropertyValueSetters = new ConcurrentDictionary<PropertyInfo, Action<T, object>>();
    }
}