using Adnc.Infra.Core.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System;

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

    public static bool IsNullableType([NotNull] this Type @this)
        => @this.IsGenericType && @this.GetGenericTypeDefinition() == typeof(Nullable<>);

    /// <summary>
    /// 根据 Type 获取默认值，实现类似 default(T) 的功能
    /// </summary>
    /// <param name="this">type</param>
    /// <returns></returns>
    public static object GetDefaultValue([NotNull] this Type @this)
        => @this.IsValueType && @this != typeof(void) ? ReflectionDictionary.TypeObejctCache.GetOrAdd(@this, Activator.CreateInstance) : null;

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

    public static MethodInfo GetMethodBySignature([NotNull] this Type @this, MethodInfo method)
    {
        var methods = @this.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name.Equals(method.Name))
            .ToArray();

        var parameterTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();
        if (method.ContainsGenericParameters)
        {
            foreach (var info in methods)
            {
                var innerParams = info.GetParameters();
                if (innerParams.Length != parameterTypes.Length)
                {
                    continue;
                }

                var idx = 0;
                foreach (var param in innerParams)
                {
                    if (!param.ParameterType.IsGenericParameter
                        && !parameterTypes[idx].IsGenericParameter
                        && param.ParameterType != parameterTypes[idx]
                    )
                    {
                        break;
                    }

                    idx++;
                }
                if (idx < parameterTypes.Length)
                {
                    continue;
                }

                return info;
            }

            return null;
        }

        var baseMethod = @this.GetMethod(method.Name, parameterTypes);
        return baseMethod;
    }

    public static MethodInfo GetBaseMethod([NotNull] this MethodInfo @this)
        => @this.DeclaringType.BaseType.GetMethodBySignature(@this);

    public static bool IsVisibleAndVirtual([NotNull] this PropertyInfo @this)
        => (@this.CanRead && @this.GetMethod.IsVisibleAndVirtual()) || (@this.CanWrite && @this.GetMethod.IsVisibleAndVirtual());

    public static bool IsVisibleAndVirtual([NotNull] this MethodInfo @this)
    {
        if (@this.IsStatic || @this.IsFinal)
            return false;

        return @this.IsVirtual &&
               (@this.IsPublic || @this.IsFamily || @this.IsFamilyOrAssembly);
    }

    public static bool IsVisible([NotNull] this MethodBase @this)
        => @this.IsPublic || @this.IsFamily || @this.IsFamilyOrAssembly;

    /// <summary>
    /// An object extension method that gets DisplayName if DisplayAttribute does not exist,return the MemberName
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The custom attribute.</returns>
    public static string GetDisplayName([NotNull] this MemberInfo @this)
        => @this.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? @this.GetCustomAttribute<DisplayAttribute>()?.Name ?? @this.Name;

    /// <summary>
    /// GetColumnName
    /// </summary>
    /// <returns></returns>
    public static string GetColumnName([NotNull] this PropertyInfo propertyInfo)
        => propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name ?? propertyInfo.Name;

    /// <summary>
    /// GetDescription
    /// </summary>
    /// <returns></returns>
    public static string GetDescription([NotNull] this MemberInfo @this)
        => @this.GetCustomAttribute<DescriptionAttribute>()?.Description ?? String.Empty;

    /// <summary>
    ///     A T extension method that searches for the specified field, using the specified
    ///     binding constraints.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The string containing the name of the data field to get.</param>
    /// <param name="bindingAttr">
    ///     A bitmask comprised of one or more BindingFlags that specify how the
    ///     search is conducted.
    /// </param>
    /// <returns>
    ///     An object representing the field that matches the specified requirements, if found;
    ///     otherwise, null.
    /// </returns>
    public static FieldInfo GetField<T>([NotNull] this T @this, string name, BindingFlags bindingAttr)
        => @this.GetType().GetField(name, bindingAttr);

    /// <summary>A T extension method that searches for the public field with the specified name.</summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The string containing the name of the data field to get.</param>
    /// <returns>
    ///     An object representing the field that matches the specified requirements, if found;
    ///     otherwise, null.
    /// </returns>
    public static FieldInfo GetField<T>([NotNull] this T @this, string name)
        => ReflectionDictionary.TypeFieldCache.GetOrAdd(@this.GetType(), t => t.GetFields()).FirstOrDefault(_ => _.Name == name);

    /// <summary>An object extension method that gets the fields.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="bindingAttr">The binding attribute.</param>
    /// <returns>An array of field information.</returns>
    public static FieldInfo[] GetFields([NotNull] this object @this, BindingFlags bindingAttr)
        => @this.GetType().GetFields(bindingAttr);

    /// <summary>An object extension method that gets the fields.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>An array of field information.</returns>
    public static FieldInfo[] GetFields([NotNull] this object @this)
        => ReflectionDictionary.TypeFieldCache.GetOrAdd(@this.GetType(), t => t.GetFields());

    /// <summary>
    ///     A T extension method that gets a field value (Public | NonPublic | Instance | Static)
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>The field value.</returns>
    public static object GetFieldValue<T>([NotNull] this T @this, string fieldName)
    {
        var field = @this.GetField(fieldName);
        return field?.GetValue(@this);
    }

    /// <summary>
    ///     A T extension method that searches for the specified method whose parameters match the specified argument
    ///     types and modifiers, using the specified binding constraints.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The string containing the name of the public method to get.</param>
    /// <param name="bindingAttr">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
    /// <returns>
    ///     An object that represents the public method with the specified name, if found; otherwise, null.
    /// </returns>
    public static MethodInfo GetMethod<T>([NotNull] this T @this, string name, BindingFlags bindingAttr)
        => @this.GetType().GetMethod(name, bindingAttr);

    /// <summary>
    ///     A T extension method that searches for the public method with the specified name.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The string containing the name of the public method to get.</param>
    /// <returns>
    ///     An object that represents the public method with the specified name, if found; otherwise, null.
    /// </returns>
    public static MethodInfo GetMethod<T>([NotNull] this T @this, string name)
        => ReflectionDictionary.TypeMethodCache.GetOrAdd(@this.GetType(), t => t.GetMethods()).FirstOrDefault(_ => _.Name == name);

    /// <summary>
    ///     A T extension method that searches for the methods defined for the current Type, using the specified binding
    ///     constraints.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="bindingAttr">A bitmask comprised of one or more BindingFlags that specify how the search is conducted.</param>
    /// <returns>
    ///     An array of MethodInfo objects representing all methods defined for the current Type that match the specified
    ///     binding constraints. or An empty array of type MethodInfo, if no methods are defined for the current Type, or
    ///     if none of the defined methods match the binding constraints.
    /// </returns>
    public static MethodInfo[] GetMethods<T>([NotNull] this T @this, BindingFlags bindingAttr)
        => @this.GetType().GetMethods(bindingAttr);

    /// <summary>
    ///     A T extension method that returns all the public methods of the current Type.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>
    ///     An array of MethodInfo objects representing all the public methods defined for the current Type. or An empty
    ///     array of type MethodInfo, if no public methods are defined for the current Type.
    /// </returns>
    public static MethodInfo[] GetMethods<T>([NotNull] this T @this)
        => ReflectionDictionary.TypeMethodCache.GetOrAdd(@this.GetType(), t => t.GetMethods());

    /// <summary>
    ///     A T extension method that gets a property.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The name.</param>
    /// <param name="bindingAttr">The binding attribute.</param>
    /// <returns>The property.</returns>
    public static PropertyInfo GetProperty<T>([NotNull] this T @this, string name, BindingFlags bindingAttr)
        => @this.GetType().GetProperty(name, bindingAttr);

    /// <summary>
    ///     A T extension method that gets a property.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The name.</param>
    /// <returns>The property.</returns>
    public static PropertyInfo GetProperty<T>([NotNull] this T @this, [NotNull] string name)
        => ReflectionDictionary.TypePropertyCache.GetOrAdd(@this.GetType(), type => type.GetProperties()).FirstOrDefault(_ => _.Name == name);

    /// <summary>A T extension method that gets property or field.</summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="name">The name.</param>
    /// <returns>The property or field.</returns>
    public static MemberInfo GetPropertyOrField<T>([NotNull] this T @this, string name)
    {
        var property = @this.GetProperty(name);
        if (property != null)
        {
            return property;
        }

        var field = @this.GetField(name);
        return field;
    }

    /// <summary>An object extension method that gets the properties.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <returns>An array of property information.</returns>
    public static PropertyInfo[] GetProperties([NotNull] this object @this)
        => ReflectionDictionary.TypePropertyCache.GetOrAdd(@this.GetType(), type => type.GetProperties());

    /// <summary>An object extension method that gets the properties.</summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="bindingAttr">The binding attribute.</param>
    /// <returns>An array of property information.</returns>
    public static PropertyInfo[] GetProperties([NotNull] this object @this, BindingFlags bindingAttr)
        => @this.GetType().GetProperties(bindingAttr);

    /// <summary>
    ///     A T extension method that gets property value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The property value.</returns>
    public static object GetPropertyValue<T>([NotNull] this T @this, string propertyName)
    {
        var property = @this.GetProperty(propertyName);

        return property?.GetValueGetter<T>()?.Invoke(@this);
    }

    /// <summary>
    ///     An object extension method that executes the method on a different thread, and waits for the result.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>An object.</returns>
    public static object InvokeMethod<T>([NotNull] this T obj, string methodName, params object[] parameters)
    {
        var type = obj.GetType();
        var method = type.GetMethod(methodName, parameters.Select(o => o.GetType()).ToArray());

        return method?.Invoke(obj, parameters);
    }

    /// <summary>
    ///     An object extension method that executes the method on a different thread, and waits for the result.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="obj">The obj to act on.</param>
    /// <param name="methodName">Name of the method.</param>
    /// <param name="parameters">Options for controlling the operation.</param>
    /// <returns>A T.</returns>
    public static T InvokeMethod<T>([NotNull] this object obj, string methodName, params object[] parameters)
    {
        var type = obj.GetType();
        var method = type.GetMethod(methodName, parameters.Select(o => o.GetType()).ToArray());

        var value = method?.Invoke(obj, parameters);
        return value.ToOrDefault<T>();
    }

    /// <summary>
    ///     An object extension method that query if '@this' is attribute defined.
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="attributeType">Type of the attribute.</param>
    /// <param name="inherit">true to inherit.</param>
    /// <returns>true if attribute defined, false if not.</returns>
    public static bool IsAttributeDefined([NotNull] this object @this, Type attributeType, bool inherit = true)
        => @this.GetType().IsDefined(attributeType, inherit);

    /// <summary>
    ///     An object extension method that query if '@this' is attribute defined.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="inherit">true to inherit.</param>
    /// <returns>true if attribute defined, false if not.</returns>
    public static bool IsAttributeDefined<T>([NotNull] this object @this, bool inherit = true) where T : Attribute
        => @this.GetType().IsDefined(typeof(T), inherit);

    /// <summary>
    ///     A T extension method that sets field value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="value">The value.</param>
    public static void SetFieldValue<T>([NotNull] this T @this, string fieldName, object value)
    {
        var type = @this.GetType();
        var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        field?.SetValue(@this, value);
    }

    /// <summary>
    ///     A T extension method that sets property value.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    public static void SetPropertyValue<T>([NotNull] this T @this, string propertyName, object value) where T : class
    {
        var property = @this.GetProperty(propertyName);
        property?.GetValueSetter()?.Invoke(@this, value);
    }

    public static Func<T, object> GetValueGetter<T>([NotNull] this PropertyInfo @this)
    {
        return StrongTypedDictionary<T>.PropertyValueGetters.GetOrAdd(@this, prop =>
        {
            if (!prop.CanRead)
                return null;

            var instance = Expression.Parameter(typeof(T), "i");
            var property = Expression.Property(instance, prop);
            var convert = Expression.TypeAs(property, typeof(object));
            return (Func<T, object>)Expression.Lambda(convert, instance).Compile();
        });
    }

    public static Func<object, object> GetValueGetter([NotNull] this PropertyInfo @this)
    {
        return ReflectionDictionary.PropertyValueGetters.GetOrAdd(@this, prop =>
        {
            if (!prop.CanRead)
                return null;

            Debug.Assert(@this.DeclaringType != null);

            var instance = Expression.Parameter(typeof(object), "obj");
            var getterCall = Expression.Call(@this.DeclaringType.IsValueType
                ? Expression.Unbox(instance, @this.DeclaringType)
                : Expression.Convert(instance, @this.DeclaringType), prop.GetGetMethod());
            var castToObject = Expression.Convert(getterCall, typeof(object));
            return (Func<object, object>)Expression.Lambda(castToObject, instance).Compile();
        });
    }

    public static Action<object, object> GetValueSetter([NotNull] this PropertyInfo @this)
    {
        return ReflectionDictionary.PropertyValueSetters.GetOrAdd(@this, prop =>
        {
            if (!prop.CanWrite)
                return null;

            var obj = Expression.Parameter(typeof(object), "o");
            var value = Expression.Parameter(typeof(object));

            Debug.Assert(@this.DeclaringType != null);

            // Note that we are using Expression.Unbox for value types and Expression.Convert for reference types
            var expr =
                Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        @this.DeclaringType.IsValueType
                            ? Expression.Unbox(obj, @this.DeclaringType)
                            : Expression.Convert(obj, @this.DeclaringType),
                        @this.GetSetMethod(),
                        Expression.Convert(value, @this.PropertyType)),
                    obj, value);
            return expr.Compile();
        });
    }

    public static Action<T, object> GetValueSetter<T>([NotNull] this PropertyInfo @this) where T : class
    {
        return StrongTypedDictionary<T>.PropertyValueSetters.GetOrAdd(@this, prop =>
        {
            if (!prop.CanWrite)
                return null;

            var instance = Expression.Parameter(typeof(T), "i");
            var argument = Expression.Parameter(typeof(object), "a");
            var setterCall = Expression.Call(instance, prop.GetSetMethod(), Expression.Convert(argument, prop.PropertyType));
            return (Action<T, object>)Expression.Lambda(setterCall, instance, argument).Compile();
        });
    }

    /// <summary>
    /// if a type has empty constructor
    /// </summary>
    /// <param name="this">type</param>
    /// <returns></returns>
    public static bool HasEmptyConstructor([NotNull] this Type @this)
        => @this.GetConstructors(BindingFlags.Instance).Any(c => c.GetParameters().Length == 0);

    /// <summary>
    /// A T extension method that query if '@this' is ValueTuple.
    /// </summary>
    /// <returns></returns>
    public static bool IsValueTuple<T>([NotNull] this T t)
        => typeof(T).IsValueTuple();

    /// <summary>
    /// A T extension method that query if '@this' is IsValueType.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool IsValueType<T>([NotNull] this T t)
        => typeof(T).IsValueType;

    /// <summary>
    ///     A T extension method that query if '@this' is array.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if array, false if not.</returns>
    public static bool IsArray<T>([NotNull] this T @this)
        => @this.GetType().IsArray;

    /// <summary>
    ///     A T extension method that query if '@this' is class.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if class, false if not.</returns>
    public static bool IsClass<T>([NotNull] this T @this)
        => @this.GetType().IsClass;

    /// <summary>
    ///     A T extension method that query if '@this' is enum.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>true if enum, false if not.</returns>
    public static bool IsEnum<T>([NotNull] this T @this)
        => typeof(T).IsEnum;

    /// <summary>
    ///     A T extension method that query if '@this' is subclass of.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <param name="type">The Type to process.</param>
    /// <returns>true if subclass of, false if not.</returns>
    public static bool IsSubclassOf<T>([NotNull] this T @this, Type type)
        => typeof(T).IsSubclassOf(type);

    /// <summary>
    ///     An Assembly extension method that gets an attribute.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>The attribute.</returns>
    public static T GetAttribute<T>([NotNull] this Assembly @this) where T : Attribute
    {
        var configAttributes = Attribute.GetCustomAttributes(@this, typeof(T), false);

        if (configAttributes != null && configAttributes.Length > 0)
        {
            return (T)configAttributes[0];
        }

        return null;
    }
}