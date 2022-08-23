namespace Adnc.Whse.Domain.EntityConfig;

public class EntityInfo : AbstractDomainEntityInfo
{
    private readonly Assembly _assembly = typeof(EntityInfo).Assembly;

    public override IEnumerable<Assembly> GetConfigAssemblys() =>
        GetConfigAssemblys(_assembly);

    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo() =>
        GetEntityTypes(_assembly).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
}