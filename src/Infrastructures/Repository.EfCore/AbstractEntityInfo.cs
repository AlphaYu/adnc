using System.Reflection;

namespace Adnc.Infra.Repository.EfCore;

public abstract class AbstractEntityInfo : IEntityInfo
{
    public virtual void OnModelCreating(dynamic modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
        if (modelBuilder is not ModelBuilder builder)
        {
            throw new InvalidOperationException(nameof(modelBuilder));
        }

        var assemblies = GetEntityAssemblies();
        foreach (var assembly in assemblies)
        {
            builder.ApplyConfigurationsFromAssembly(assembly);
        }

        var entityTypes = GetEntityTypes(assemblies);
        foreach (var entityType in entityTypes)
        {
            builder.Entity(entityType, entityTypeBuilder =>
            {
                var metadata = entityTypeBuilder.Metadata;
                var tableComment = metadata.ClrType.GetSummary();
                entityTypeBuilder.ToTable(t => t.HasComment(tableComment));

                var properties = metadata.GetProperties();
                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var memberComment = metadata.ClrType?.GetMember(propertyName)?.FirstOrDefault()?.GetSummary();
                    entityTypeBuilder.Property(propertyName).HasComment(memberComment);
                }
            });
        }

        SetTableName(modelBuilder);
    }

    protected abstract List<Assembly> GetEntityAssemblies();

    protected virtual void SetTableName(dynamic modelBuilder)
    {
    }

    protected virtual List<Type> GetEntityTypes(IEnumerable<Assembly> assemblies)
    {
        var typeList = assemblies?.SelectMany(assembly => assembly.GetTypes()
                                                 .Where(m => m.FullName != null
                                                 && typeof(EfEntity).IsAssignableFrom(m)
                                                 && !m.IsAbstract));

        return typeList?.ToList() ?? [];
    }
}
