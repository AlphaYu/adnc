using System.Runtime.CompilerServices;

namespace System;

public static class ReflectionExtension
{
    public static bool IsNotAbstractClass(this Type type, bool publicOnly)
    {
        if (type.IsSpecialName)
            return false;

        if (type.IsClass && !type.IsAbstract)
        {
            if (type.HasAttribute<CompilerGeneratedAttribute>())
                return false;

            if (publicOnly)
                return type.IsPublic || type.IsNestedPublic;

            return true;
        }
        return false;
    }

    public static bool HasAttribute(this Type type, Type attributeType) => type.IsDefined(attributeType, inherit: true);

    public static bool HasAttribute<T>(this Type type) where T : Attribute => type.HasAttribute(typeof(T));

    public static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute => type.GetCustomAttributes<T>(inherit: true).Any(predicate);
}