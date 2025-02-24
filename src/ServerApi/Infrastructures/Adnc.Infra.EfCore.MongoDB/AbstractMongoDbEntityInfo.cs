using System.Reflection;

namespace Adnc.Infra.Repository.EfCore.MongoDB;

public abstract class AbstractMongoDbEntityInfo : IEntityInfo
{
    public virtual void OnModelCreating(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
            throw new ArgumentNullException(nameof(modelBuilder));

        var assemblies = GetCurrentAssemblies();

        var entityTypes = GetEntityTypes(assemblies);
        entityTypes.ForEach(entityType => builder.Entity(entityType));

        SetCollectionName(modelBuilder);
    }

    protected abstract List<Assembly> GetCurrentAssemblies();

    protected virtual void SetCollectionName(dynamic modelBuilder)
    {
    }

    protected virtual List<Type> GetEntityTypes(IEnumerable<Assembly> assemblies)
    {
        if (assemblies is null)
            throw new ArgumentNullException(nameof(assemblies));

        var typeList = assemblies.SelectMany(assembly => assembly.GetTypes()
                                                 .Where(m => m.FullName != null
                                                 && typeof(MongoEntity).IsAssignableFrom(m)
                                                 && !m.IsAbstract));

        return typeList.ToList() ?? new List<Type>();
    }
}