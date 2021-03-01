using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities;

namespace Adnc.Ord.Domain.Entities.Config
{
    public class OrderConfig : EntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.CustomerId).IsRequired();

            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,4)");

            builder.Property(x => x.Remark).HasMaxLength(64);

            builder.OwnsOne(x => x.Status, y =>
            {
                y.Property(y => y.Code).IsRequired().HasColumnName("StatusCode");
                y.Property(y => y.ChangesReason).HasColumnName("StatusChangesReason").HasMaxLength(32);
            });


            builder.OwnsOne(x => x.Receiver, y =>
            {
                y.Property(y => y.Name).IsRequired().HasColumnName("ReceiverName").HasMaxLength(16);
                y.Property(y => y.Phone).IsRequired().HasColumnName("ReceiverPhone").HasMaxLength(11);
                y.Property(y => y.Address).IsRequired().HasColumnName("ReceiverAddress").HasMaxLength(64);
            });

            builder.HasMany(x => x.Items).WithOne().HasForeignKey(y => y.OrderId);
        }
    }
}
