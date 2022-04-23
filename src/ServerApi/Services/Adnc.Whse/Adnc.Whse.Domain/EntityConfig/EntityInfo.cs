namespace Adnc.Whse.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    public override (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo()
    {
        var assembly = this.GetType().Assembly;
        var entityTypes = base.GetEntityTypes(assembly);

        return (assembly, entityTypes);
    }
}