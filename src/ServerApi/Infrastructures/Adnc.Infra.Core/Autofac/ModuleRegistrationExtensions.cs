namespace Autofac;

public static class ModuleRegistrationExtensions
{
    private static readonly ConcurrentDictionary<string, int> s_KeyValues = new ConcurrentDictionary<string, int>();

    public static IModuleRegistrar RegisterModuleIfNotRegistered(this ContainerBuilder builder, IModule module)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (module == null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        string modelName = module.GetType().FullName;
        if (s_KeyValues.ContainsKey(modelName))
            return builder.RegisterModule<NullModel>();

        if (s_KeyValues.TryAdd(modelName, 1))
            return builder.RegisterModule(module);

        throw new ArgumentException($"autofac register module fail:{modelName}");
    }

    public static IModuleRegistrar RegisterModuleIfNotRegistered<TModule>(this ContainerBuilder builder)
    where TModule : IModule, new()
    {
        return builder.RegisterModuleIfNotRegistered(new TModule());
    }
}