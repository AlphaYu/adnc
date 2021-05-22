using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Core.Shared.Entities.Config
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
       where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var entityType = typeof(TEntity);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            if (typeof(IConcurrency).IsAssignableFrom(entityType))
            {
                builder.Property("RowVersion").IsRequired().IsRowVersion().ValueGeneratedOnAddOrUpdate();
            }

            if (typeof(ISoftDelete).IsAssignableFrom(entityType))
            {
                builder.Property("IsDeleted")
                       .HasDefaultValue(false);
                builder.HasQueryFilter(d => EF.Property<bool>(d, "IsDeleted") == false);
            }
        }
    }
}