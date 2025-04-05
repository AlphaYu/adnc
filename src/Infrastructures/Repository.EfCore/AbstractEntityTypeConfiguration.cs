namespace Adnc.Infra.Repository.EfCore;

public abstract class AbstractEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
   where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        var entityType = typeof(TEntity);
        ConfigureKey(builder, entityType);
        ConfigureConcurrency(builder, entityType);
        ConfigureSoftDelete(builder, entityType);
        ConfigureAuditInfo(builder, entityType);
    }

    protected virtual void ConfigureKey(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnOrder(1).ValueGeneratedNever();
    }

    protected virtual void ConfigureConcurrency(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (typeof(IConcurrency).IsAssignableFrom(entityType))
        {
            builder.Property($"{nameof(IConcurrency.RowVersion)}").IsRequired().IsRowVersion().ValueGeneratedOnAddOrUpdate().HasColumnOrder(98);
        }
    }

    protected virtual void ConfigureSoftDelete(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        var filedName = nameof(ISoftDelete.IsDeleted);
        if (typeof(ISoftDelete).IsAssignableFrom(entityType))
        {
            builder.Property(filedName)
                       .HasDefaultValue(false)
                       .HasColumnOrder(99);
            builder.HasQueryFilter(d => !EF.Property<bool>(d, filedName));
        }
    }

    protected virtual void ConfigureAuditInfo(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (typeof(IBasicAuditInfo).IsAssignableFrom(entityType))
        {
            builder.Property($"{nameof(IFullAuditInfo.CreateBy)}").HasColumnOrder(100);
            builder.Property($"{nameof(IFullAuditInfo.CreateTime)}").HasColumnOrder(101);
        }

        if (typeof(IFullAuditInfo).IsAssignableFrom(entityType))
        {
            builder.Property($"{nameof(IFullAuditInfo.ModifyBy)}").HasColumnOrder(102);
            builder.Property($"{nameof(IFullAuditInfo.ModifyTime)}").HasColumnOrder(103);
        }
    }
}
