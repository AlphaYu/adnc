namespace Adnc.Infra.Entities;

public interface IEntityInfo
{
    IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo();

    IEnumerable<Assembly> GetConfigAssemblys();
}

public class EntityTypeInfo
{
    public Type Type { get; set; } = default!;

    public IEnumerable<object>? DataSeeding { get; set; }
}