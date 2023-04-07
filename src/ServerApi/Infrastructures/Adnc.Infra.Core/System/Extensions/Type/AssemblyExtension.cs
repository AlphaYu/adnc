namespace System;

public static class AssemblyExtension
{
    public static IEnumerable<Type> GetImplementationTypesWithOutAbstractClass<TServiceType>(this Assembly assembly)
        where TServiceType : class
    {
        var implTypes = GetImplementationTypes<TServiceType>(assembly).Where(type => type.IsNotAbstractClass(true));
        return implTypes ?? Array.Empty<Type>();
    }

    public static IEnumerable<Type> GetImplementationTypes<TServiceType>(this Assembly assembly)
    where TServiceType : class
    {
        var serviceType = typeof(TServiceType);
        var implTypes = assembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType));
        return implTypes ?? Array.Empty<Type>();
    }
}