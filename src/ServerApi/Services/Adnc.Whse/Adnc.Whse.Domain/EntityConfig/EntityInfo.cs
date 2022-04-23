namespace Adnc.Whse.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo()
    {
        return base.GetEntityTypes(this.GetType().Assembly).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
    }
}