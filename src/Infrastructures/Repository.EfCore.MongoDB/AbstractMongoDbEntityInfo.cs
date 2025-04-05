using System.Reflection;

namespace Adnc.Infra.Repository.EfCore.MongoDB;

public abstract class AbstractMongoDbEntityInfo : IEntityInfo
{
    public virtual void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));

        var assemblies = GetEntityAssemblies();
        var entityTypes = GetEntityTypes(assemblies);
        foreach (var entityType in entityTypes)
        {
            modelBuilder.Entity(entityType);
        }

        SetCollectionName(modelBuilder);
    }

    protected abstract List<Assembly> GetEntityAssemblies();

    protected virtual void SetCollectionName(ModelBuilder modelBuilder)
    {
    }

    protected virtual List<Type> GetEntityTypes(IEnumerable<Assembly> assemblies)
    {
        var typeList = assemblies?.SelectMany(assembly => assembly.GetTypes()
                                                 .Where(m => m.FullName != null
                                                 && typeof(MongoEntity).IsAssignableFrom(m)
                                                 && !m.IsAbstract));

        return typeList?.ToList() ?? [];
    }
}
