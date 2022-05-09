namespace Adnc.Infra.Entities;

public interface IEntityInfo
{
    IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo();
}

public class EntityTypeInfo
{
    public Type? Type { get; set; }

    public IEnumerable<object>? DataSeeding { get; set; }
}