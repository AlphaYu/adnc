namespace Adnc.Shared.Domain.Entities;

public abstract class AbstractDomainEntityInfo : AbstracEntityInfo, IEntityInfo
{
    protected override List<Type> GetEntityTypes(IEnumerable<Assembly> assemblies)
    {
        var typeList = assemblies.SelectMany(assembly => assembly.GetTypes()
                                                  .Where(m => m.FullName != null
                                                   && (typeof(AggregateRoot).IsAssignableFrom(m) || typeof(DomainEntity).IsAssignableFrom(m))
                                                   && !m.IsAbstract));

        if (typeList is null)
            typeList = new List<Type>();

        return typeList.Append(typeof(EventTracker)).ToList();
    }
}