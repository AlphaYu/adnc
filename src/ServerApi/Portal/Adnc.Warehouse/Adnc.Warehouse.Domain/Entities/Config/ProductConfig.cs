using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Warehouse.Domain.Entities.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(x => x.Describe)
                   .HasMaxLength(128);

            builder.Property(x => x.Sku)
                   .IsRequired()
                   .HasMaxLength(32);

            builder.OwnsOne(x => x.Status)
                   .Property(x => x.StatusCode)
                   .IsRequired()
                   .HasColumnName("StatusCode");

            builder.OwnsOne(x => x.Status)
                   .Property(x => x.ChangeStatusReason)
                   .HasColumnName("ChangeStatusReason")
                   .HasMaxLength(32);

            builder.Property(x => x.Unit)
                   .IsRequired()
                   .HasMaxLength(4);
           
        }
    }
}
