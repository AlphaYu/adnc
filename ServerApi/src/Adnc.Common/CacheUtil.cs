using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Adnc.Common
{
    public static class CacheUtil
    {
        /// <summary>
        /// TypePropertyCache
        /// </summary>
        public static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertyCache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        internal static readonly ConcurrentDictionary<Type, FieldInfo[]> TypeFieldCache = new ConcurrentDictionary<Type, FieldInfo[]>();

        internal static readonly ConcurrentDictionary<Type, MethodInfo[]> TypeMethodCache = new ConcurrentDictionary<Type, MethodInfo[]>();

        //internal static readonly ConcurrentDictionary<Type, Func<ServiceContainer, object>> TypeNewFuncCache = new ConcurrentDictionary<Type, Func<ServiceContainer, object>>();

        internal static readonly ConcurrentDictionary<Type, ConstructorInfo> TypeConstructorCache = new ConcurrentDictionary<Type, ConstructorInfo>();

        internal static readonly ConcurrentDictionary<Type, Func<object>> TypeEmptyConstructorFuncCache = new ConcurrentDictionary<Type, Func<object>>();

        internal static readonly ConcurrentDictionary<Type, Func<object[], object>> TypeConstructorFuncCache = new ConcurrentDictionary<Type, Func<object[], object>>();

        internal static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> PropertyValueGetters = new ConcurrentDictionary<PropertyInfo, Func<object, object>>();

        internal static readonly ConcurrentDictionary<PropertyInfo, Action<object, object>> PropertyValueSetters = new ConcurrentDictionary<PropertyInfo, Action<object, object>>();
    }

    internal static class StrongTypedCache<T>
    {
        public static ConcurrentDictionary<PropertyInfo, Func<T, object>> PropertyValueGetters = new ConcurrentDictionary<PropertyInfo, Func<T, object>>();

        public static ConcurrentDictionary<PropertyInfo, Action<T, object>> PropertyValueSetters = new ConcurrentDictionary<PropertyInfo, Action<T, object>>();
    }
}
