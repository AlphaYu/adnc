namespace Adnc.Shared.Repository.EfEntities.Config;

public abstract class AbstractEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
   where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        var entityType = typeof(TEntity);
        ConfigureKey(builder, entityType);
        ConfigureConcurrency(builder, entityType);
        ConfigureQueryFilter(builder, entityType);
    }

    protected virtual void ConfigureKey(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnOrder(1).ValueGeneratedNever();
    }

    protected virtual void ConfigureConcurrency(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (typeof(IConcurrency).IsAssignableFrom(entityType))
            builder.Property("RowVersion").IsRequired().IsRowVersion().ValueGeneratedOnAddOrUpdate();
    }

    protected virtual void ConfigureQueryFilter(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(entityType))
        {
            builder.Property("IsDeleted")
                       .HasDefaultValue(false)
                       .HasColumnOrder(2);
            builder.HasQueryFilter(d => !EF.Property<bool>(d, "IsDeleted"));
        }
    }
}