namespace Adnc.Maint.Entities;

public class EntityInfo : AbstracSharedEntityInfo
{
    private readonly Assembly _assembly = typeof(EntityInfo).Assembly;

    public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo() =>
        GetEntityTypes(_assembly, false).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });


    public override IEnumerable<Assembly> GetConfigAssemblys() =>
        GetConfigAssemblys(_assembly, false);
}