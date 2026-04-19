using Adnc.Demo.Whse.Domain.Aggregates.ProductAggregate;

namespace Adnc.Demo.Whse.Domain.EntityConfig;

public class ProductConfig : AbstractEntityTypeConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(Product.Name_MaxLength);

        builder.Property(x => x.Describe)
               .HasMaxLength(Product.Describe_MaxLength);

        builder.Property(x => x.Price)
               .IsRequired()
               .HasColumnType("decimal(18,4)");

        builder.Property(x => x.Sku)
               .IsRequired()
               .HasMaxLength(Product.Sku_MaxLength);

        builder.OwnsOne(x => x.Status, y =>
        {
            y.Property(x => x.Code).IsRequired().HasColumnName("statuscode");
            y.Property(x => x.ChangesReason).HasColumnName("statuschangesreason").HasMaxLength(Product.ChangesReason_MaxLength);
        });

        builder.Property(x => x.Unit)
               .IsRequired()
               .HasMaxLength(Product.Unit_MaxLength);
    }
}
