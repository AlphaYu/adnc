using Adnc.Infra.Entities.Config;
using Adnc.Shared.Consts.Entity.Whse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Whse.Domain.Entities.Config
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(ProductConts.Name_MaxLength);

            builder.Property(x => x.Describe)
                   .HasMaxLength(ProductConts.Describe_MaxLength);

            builder.Property(x => x.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,4)");

            builder.Property(x => x.Sku)
                   .IsRequired()
                   .HasMaxLength(ProductConts.Sku_MaxLength);

            builder.OwnsOne(x => x.Status, y =>
            {
                y.Property(x => x.Code).IsRequired().HasColumnName("StatusCode");
                y.Property(x => x.ChangesReason).HasColumnName("StatusChangesReason").HasMaxLength(ProductConts.ChangesReason_MaxLength);
            });

            builder.Property(x => x.Unit)
                   .IsRequired()
                   .HasMaxLength(ProductConts.Unit_MaxLength);
        }
    }
}