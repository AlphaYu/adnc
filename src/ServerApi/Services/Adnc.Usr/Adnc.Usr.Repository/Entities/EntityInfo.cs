using System.Reflection;

namespace Adnc.Usr.Entities;

public class EntityInfo : AbstractEntityInfo
{
    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo()
    {
        return GetEntityTypes(this.GetType().Assembly).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
    }

    public override IEnumerable<Assembly> GetConfigAssemblys()
    {
        return new List<Assembly> { this.GetType().Assembly };
    }
}