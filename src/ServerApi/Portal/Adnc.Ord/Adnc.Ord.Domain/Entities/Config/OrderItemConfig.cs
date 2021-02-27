using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities;

namespace Adnc.Ord.Domain.Entities.Config
{
    public class OrderItemConfig : EntityTypeConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.OrderId).IsRequired();

            builder.OwnsOne(x => x.Product, y =>
             {
                 y.Property(y => y.Id).IsRequired().HasColumnName("ProductId");
                 y.Property(y => y.Name).IsRequired().HasMaxLength(64).HasColumnName("ProductName");
                 y.Property(y => y.Price).IsRequired().HasColumnName("ProductPrice").HasColumnType("decimal(18,4)");
             });

            builder.Property(x => x.Count).IsRequired();

            //builder.Property(x => x.Amount)
            //       .HasComputedColumnSql("[ProductPrice]*[Count]");
        }
    }
}
