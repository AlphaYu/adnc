namespace Adnc.Cus.Entities;

public class EntityInfo : Shared.Entities.AbstracSharedEntityInfo
{
    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo()
    {
        var assembly = typeof(EntityInfo).Assembly;
        var typeList = GetEntityTypes(assembly);
        return typeList.Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
    }
}