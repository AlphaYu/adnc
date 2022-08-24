namespace Adnc.Shared.Repository.EfEntities;

public abstract class AbstracSharedEntityInfo : IEntityInfo
{
    public abstract IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo();

    public abstract IEnumerable<Assembly> GetConfigAssemblys();

    protected virtual IEnumerable<Type> GetEntityTypes(Assembly assembly, bool containsSharedType = true)
    {
        var typeList = assembly.GetTypes().Where(m =>
                                                   m.FullName != null
                                                   && typeof(EfEntity).IsAssignableFrom(m)
                                                   && !m.IsAbstract);
        if (typeList is null)
            typeList = new List<Type>();

        return containsSharedType ? typeList.Append(typeof(EventTracker)) : typeList;
    }

    protected virtual IEnumerable<Assembly> GetConfigAssemblys(Assembly assembly, bool containsSharedAssembly = true)
    {
        var assemblies = new List<Assembly> { assembly };
        return containsSharedAssembly ? assemblies.Append(typeof(EventTracker).Assembly) : assemblies;
    }
}