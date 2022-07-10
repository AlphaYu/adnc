namespace Adnc.Shared.Entities;

public abstract class AbstracSharedEntityInfo : AbstractEntityInfo
{
    protected override IEnumerable<Type> GetEntityTypes(Assembly assembly)
    {
        var typeList = base.GetEntityTypes(assembly) ?? new List<Type>();
        return typeList.Append(typeof(EventTracker));
    }
}