using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Orders.Domain.Entities.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            builder.Property(e => e.RowVersion)
                   .IsConcurrencyToken()
                   .HasColumnType("timestamp(3)")
                   .HasDefaultValueSql("'2000-07-01 22:33:02.559'")
                   .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.CustomerId)
                   .IsRequired();

            builder.Property(x => x.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,4)");

            builder.Property(x => x.Remark)
                   .HasMaxLength(64);

            builder.OwnsOne(x => x.Status)
                   .Property(y => y.StatusCode)
                   .IsRequired()
                   .HasColumnName("StatusCode");

            builder.OwnsOne(x => x.Status)
                   .Property(y => y.ChangeStatusReason)
                   .HasColumnName("ChangeStatusReason")
                   .HasMaxLength(32);

            builder.OwnsOne(x => x.DeliveryInfomaton)
                   .Property(y => y.Name)
                   .IsRequired()
                   .HasColumnName("DeliveryName")
                   .HasMaxLength(16);

            builder.OwnsOne(x => x.DeliveryInfomaton)
                   .Property(y => y.Phone)
                   .IsRequired()
                   .HasColumnName("DeliveryPhone")
                   .HasMaxLength(11);

            builder.OwnsOne(x => x.DeliveryInfomaton)
                   .Property(y => y.Address)
                   .IsRequired()
                   .HasColumnName("DeliveryAddress")
                   .HasMaxLength(64);

            builder.HasMany(x => x.Items)
                   .WithOne()
                   .HasForeignKey(y => y.OrderId);
        }
    }
}
