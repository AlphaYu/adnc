namespace Adnc.Shared.Domain.Entities;

public abstract class AbstractDomainEntityInfo : IEntityInfo
{
    public abstract IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo();

    protected virtual IEnumerable<Type> GetEntityTypes(Assembly assembly)
    {
        var typeList = assembly.GetTypes().Where(m =>
                                                   m.FullName != null
                                                   && (typeof(AggregateRoot).IsAssignableFrom(m) || typeof(DomainEntity).IsAssignableFrom(m))
                                                   && !m.IsAbstract);
        if (typeList is null)
            typeList = new List<Type>();

        return typeList.Append(typeof(EventTracker));
    }
}