using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Core.Shared.Entities
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
       where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var entityType = typeof(TEntity);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            if (entityType is IConcurrency)
            {
                builder.Property<DateTime>("RowVersion")
                       .IsRowVersion()
                       .IsConcurrencyToken()
                       .HasColumnType("timestamp(3)")
                       .HasDefaultValueSql("'2000-07-01 22:33:02.559'")
                       .ValueGeneratedOnAddOrUpdate();
            }

            if (entityType is ISoftDelete)
            {
                builder.Property<bool>("IsDeleted")
                       .HasDefaultValue(false);
                builder.HasQueryFilter(d => EF.Property<bool>(d, "IsDeleted") == false);
            }

            if (entityType is IBasicAuditInfo)
            {
                builder.Property<long?>("CreateBy");

                builder.Property<DateTime?>("CreateTime")
                       .HasColumnType("timestamp(3)");
            }

            if (entityType is IFullAuditInfo)
            {
                builder.Property<long?>("ModifyBy");

                builder.Property<DateTime?>("ModifyTime")
                       .HasColumnType("timestamp(3)");
            }
        }
    }
}
