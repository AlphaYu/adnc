using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Orders.Domain.Entities.Config
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            builder.Property(x => x.OrderId)
                   .IsRequired();

            builder.OwnsOne(x => x.Product)
                   .Property(y => y.Id)
                   .IsRequired()
                   .HasColumnName("ProductId");

            builder.OwnsOne(x => x.Product)
                   .Property(y => y.Name)
                   .IsRequired()
                   .HasMaxLength(64)
                   .HasColumnName("ProductName");

            builder.OwnsOne(x => x.Product)
                   .Property(y => y.Price)
                   .IsRequired()
                   .HasColumnName("ProductPrice");

            builder.Property(x => x.Count)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .HasComputedColumnSql("[ProductPrice]*[Count]");
        }
    }
}
