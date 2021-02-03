using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Warehouse.Core.Entities.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(x => x.Describe)
                   .HasMaxLength(128);

            builder.Property(x => x.Sku)
                   .IsRequired()
                   .HasMaxLength(32);

            builder.Property(x => x.Status.StatusCode)
                   .IsRequired()
                   .HasColumnName("StatusCode");
            builder.Property(x => x.Status.ChangeStatusReason)
                   .HasMaxLength(32);

            builder.Property(x => x.Unit)
                   .IsRequired()
                   .HasMaxLength(4);
        }
    }
}
