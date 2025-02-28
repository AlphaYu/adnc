using Adnc.Infra.Core.Internal;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System;

public static class TypeExtension
{
    public static T? CreateInstance<T>(this Type type)
    {
        var obj = Activator.CreateInstance(type);
        if (obj is null)
            return default;
        return (T)obj;
    }

    public static T? CreateInstance<T>(this Type type, params object[] args)
    {
        var obj = Activator.CreateInstance(type, args);
        if (obj is null)
            return default;
        return (T)obj;
    }

    public static bool IsNullableType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public static bool IsVisibleAndVirtual([NotNull] this MethodInfo methodInfo)
    {
        if (methodInfo.IsStatic || methodInfo.IsFinal)
            return false;

        return methodInfo.IsVirtual &&
               (methodInfo.IsPublic || methodInfo.IsFamily || methodInfo.IsFamilyOrAssembly);
    }

    public static bool IsVisible(this MethodBase methodBase) => methodBase.IsPublic || methodBase.IsFamily || methodBase.IsFamilyOrAssembly;

    public static Func<T, object> GetValueGetter<T>(this PropertyInfo propertyInfo)
    {
        return StrongTypedDictionary<T>.PropertyValueGetters.GetOrAdd(propertyInfo, prop =>
        {
            if (!prop.CanRead)
                return (x) => string.Empty;

            var instance = Expression.Parameter(typeof(T), "i");
            var property = Expression.Property(instance, prop);
            var convert = Expression.TypeAs(property, typeof(object));
            return (Func<T, object>)Expression.Lambda(convert, instance).Compile();
        });
    }

    public static Func<object, object> GetValueGetter(this PropertyInfo propertyInfo)
    {
        return ReflectionDictionary.PropertyValueGetters.GetOrAdd(propertyInfo, prop =>
        {
            if (!prop.CanRead)
                return (x) => string.Empty;

            Debug.Assert(propertyInfo.DeclaringType != null);

            var method = prop.GetGetMethod();
            if (method is null)
                return (x) => string.Empty;

            var instance = Expression.Parameter(typeof(object), "obj");
            var getterCall = Expression.Call(propertyInfo.DeclaringType.IsValueType
                ? Expression.Unbox(instance, propertyInfo.DeclaringType)
                : Expression.Convert(instance, propertyInfo.DeclaringType), method);
            var castToObject = Expression.Convert(getterCall, typeof(object));
            return (Func<object, object>)Expression.Lambda(castToObject, instance).Compile();
        });
    }

    public static Action<object, object> GetValueSetter(this PropertyInfo propertyInfo)
    {
        return ReflectionDictionary.PropertyValueSetters.GetOrAdd(propertyInfo, prop =>
        {
            if (!prop.CanWrite)
                return (x, y) => { };

            var obj = Expression.Parameter(typeof(object), "o");
            var value = Expression.Parameter(typeof(object));

            Debug.Assert(propertyInfo.DeclaringType != null);

            var method = propertyInfo.GetSetMethod();
            if (method is null)
                return (x, y) => { };

            // Note that we are using Expression.Unbox for value types and Expression.Convert for reference types
            var expr =
                Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        propertyInfo.DeclaringType.IsValueType
                            ? Expression.Unbox(obj, propertyInfo.DeclaringType)
                            : Expression.Convert(obj, propertyInfo.DeclaringType),
                        method,
                        Expression.Convert(value, propertyInfo.PropertyType)),
                    obj, value);
            return expr.Compile();
        });
    }

    public static Action<T, object> GetValueSetter<T>([NotNull] this PropertyInfo @this) where T : class
    {
        return StrongTypedDictionary<T>.PropertyValueSetters.GetOrAdd(@this, prop =>
        {
            if (!prop.CanWrite)
                return (x, y) => { };

            var method = prop.GetGetMethod();
            if (method is null)
                return (x, y) => { };

            var instance = Expression.Parameter(typeof(T), "i");
            var argument = Expression.Parameter(typeof(object), "a");
            var setterCall = Expression.Call(instance, method, Expression.Convert(argument, prop.PropertyType));
            return (Action<T, object>)Expression.Lambda(setterCall, instance, argument).Compile();
        });
    }

    public static bool HasEmptyConstructor([NotNull] this Type @this) => @this.GetConstructors(BindingFlags.Instance).Any(c => c.GetParameters().Length == 0);
}