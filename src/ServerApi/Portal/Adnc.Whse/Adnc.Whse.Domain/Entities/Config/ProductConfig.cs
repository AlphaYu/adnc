using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;

namespace Adnc.Whse.Domain.Entities.Config
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(x => x.Describe)
                   .HasMaxLength(128);

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,4)");

            builder.Property(x => x.Sku)
                   .IsRequired()
                   .HasMaxLength(32);

            builder.OwnsOne(x => x.Status, y =>
            {
                y.Property(x => x.Code).IsRequired().HasColumnName("StatusCode");
                y.Property(x => x.ChangesReason).HasColumnName("StatusChangesReason").HasMaxLength(32);
            });


            builder.Property(x => x.Unit)
                   .IsRequired()
                   .HasMaxLength(4);
        }
    }
}
