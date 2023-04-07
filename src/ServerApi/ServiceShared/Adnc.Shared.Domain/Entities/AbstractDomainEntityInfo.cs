namespace Adnc.Shared.Domain.Entities;

public abstract class AbstractDomainEntityInfo : AbstracSharedEntityInfo
{
    protected AbstractDomainEntityInfo(UserContext userContext) : base(userContext)
    {
    }

    protected  override IEnumerable<Type> GetEntityTypes(Assembly assembly)
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