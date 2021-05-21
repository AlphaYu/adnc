using JetBrains.Annotations;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Adnc.Infra.Common.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        ///     A Type extension method that creates an instance.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The new instance.</returns>
        public static T CreateInstance<T>([NotNull] this Type @this) => (T)Activator.CreateInstance(@this);

        /// <summary>
        ///     A Type extension method that creates an instance.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The new instance.</returns>
        public static T CreateInstance<T>([NotNull] this Type @this, params object[] args) => (T)Activator.CreateInstance(@this, args);

        /// <summary>
        /// if a type has empty constructor
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static bool HasEmptyConstructor([NotNull] this Type type)
            => type.GetConstructors(BindingFlags.Instance).Any(c => c.GetParameters().Length == 0);

        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static readonly ConcurrentDictionary<Type, object> _defaultValues =
            new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 根据 Type 获取默认值，实现类似 default(T) 的功能
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static object GetDefaultValue([NotNull] this Type type) =>
            type.IsValueType && type != typeof(void) ? _defaultValues.GetOrAdd(type, Activator.CreateInstance) : null;

        /// <summary>
        /// GetUnderlyingType if nullable else return self
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static Type Unwrap([NotNull] this Type type)
            => Nullable.GetUnderlyingType(type) ?? type;

        /// <summary>
        /// GetUnderlyingType
        /// </summary>
        /// <param name="type">type</param>
        /// <returns></returns>
        public static Type GetUnderlyingType([NotNull] this Type type)
            => Nullable.GetUnderlyingType(type);
    }
}