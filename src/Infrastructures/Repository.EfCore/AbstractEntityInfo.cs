using System.Reflection;

namespace Adnc.Infra.Repository.EfCore;

public abstract class AbstractEntityInfo : IEntityInfo
{
    public virtual void OnModelCreating(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        var assemblies = GetCurrentAssemblies();
        assemblies.ForEach(assembly => builder.ApplyConfigurationsFromAssembly(assembly));

        var entityTypes = GetEntityTypes(assemblies);
        entityTypes.ForEach(entityType => builder.Entity(entityType));

        SetComment(modelBuilder, entityTypes);

        SetTableName(modelBuilder);
    }

    protected abstract List<Assembly> GetCurrentAssemblies();

    protected virtual void SetTableName(dynamic modelBuilder)
    {
    }

    private static void SetComment(ModelBuilder modelBuilder, IEnumerable<Type>? types)
    {
        if (types is null)
        {
            return;
        }

        var entityTypes = modelBuilder.Model.GetEntityTypes().Where(x => types.Contains(x.ClrType));
        entityTypes.ForEach(entityType =>
        {
            modelBuilder.Entity(entityType.Name, buider =>
            {
                var typeSummary = entityType.ClrType.GetSummary();
                buider.ToTable(t => t.HasComment(typeSummary));
                //buider.HasComment(typeSummary);

                entityType.GetProperties().ForEach(property =>
                {
                    var propertyName = property.Name;
                    var memberSummary = entityType.ClrType?.GetMember(propertyName)?.FirstOrDefault()?.GetSummary();
                    buider.Property(propertyName).HasComment(memberSummary);
                });
            });
        });
    }

    protected virtual List<Type> GetEntityTypes(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        var typeList = assemblies.SelectMany(assembly => assembly.GetTypes()
                                                 .Where(m => m.FullName != null
                                                 && typeof(EfEntity).IsAssignableFrom(m)
                                                 && !m.IsAbstract));

        return typeList.ToList() ?? [];
    }
}