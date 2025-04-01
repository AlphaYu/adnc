using System.Reflection;

namespace Adnc.Infra.Repository.EfCore.MongoDB;

public abstract class AbstractMongoDbEntityInfo : IEntityInfo
{
    public virtual void OnModelCreating(dynamic modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
        if (modelBuilder is not ModelBuilder builder)
        {
            throw new InvalidOperationException(nameof(modelBuilder));
        }

        var assemblies = GetEntityAssemblies();
        var entityTypes = GetEntityTypes(assemblies);
        foreach (var entityType in entityTypes)
        {
            builder.Entity(entityType);
        }

        SetCollectionName(modelBuilder);
    }

    protected abstract List<Assembly> GetEntityAssemblies();

    protected virtual void SetCollectionName(dynamic modelBuilder)
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
