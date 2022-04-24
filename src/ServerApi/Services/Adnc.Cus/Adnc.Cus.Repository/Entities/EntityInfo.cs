namespace Adnc.Cus.Entities;

public class EntityInfo : AbstractEntityInfo
{
    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo()
    {
        return base.GetEntityTypes(this.GetType().Assembly).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
    }
}