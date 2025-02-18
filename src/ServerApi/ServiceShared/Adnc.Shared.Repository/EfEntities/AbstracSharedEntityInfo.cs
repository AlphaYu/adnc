namespace Adnc.Shared.Repository.EfEntities;

public abstract class AbstracSharedEntityInfo : IEntityInfo
{
    protected abstract Assembly GetCurrentAssembly();

    protected virtual IEnumerable<Type> GetEntityTypes(Assembly assembly)
    {
        var typeList = assembly.GetTypes().Where(m =>
                                                   m.FullName != null
                                                   && typeof(EfEntity).IsAssignableFrom(m)
                                                   && !m.IsAbstract);
        if (typeList is null)
            typeList = new List<Type>();

        return typeList.Append(typeof(EventTracker));
    }

    protected virtual void SetComment(ModelBuilder modelBuilder, IEnumerable<Type>? types)
    {
        if (types is null)
            return;

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
                    string propertyName = property.Name;
                    var memberSummary = entityType.ClrType?.GetMember(propertyName)?.FirstOrDefault()?.GetSummary();
                    buider.Property(propertyName).HasComment(memberSummary);
                });
            });
        });
    }

    protected virtual void SetTableName(dynamic modelBuilder)
    {
    }

    public virtual void OnModelCreating(dynamic modelBuilder)
    {
        if (modelBuilder is not ModelBuilder builder)
            throw new ArgumentNullException(nameof(modelBuilder));

        var entityAssembly = GetCurrentAssembly();
        var assemblies = new List<Assembly> { entityAssembly, typeof(EventTracker).Assembly };

        var entityTypes = GetEntityTypes(entityAssembly);
        entityTypes?.ForEach(t => builder.Entity(t));

        assemblies?.ForEach(assembly => builder.ApplyConfigurationsFromAssembly(assembly));

        SetComment(modelBuilder, entityTypes);

        SetTableName(modelBuilder);
    }
}