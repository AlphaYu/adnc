using System.Reflection;

namespace Adnc.Whse.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    public override IEnumerable<Assembly> GetConfigAssemblys()
    {
        return GetConfigAssemblys(this.GetType().Assembly);
    }

    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo()
    {
        return GetEntityTypes(this.GetType().Assembly).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
    }
}