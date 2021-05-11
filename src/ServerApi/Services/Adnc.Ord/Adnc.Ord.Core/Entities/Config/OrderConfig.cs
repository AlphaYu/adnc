using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Ord;

namespace Adnc.Ord.Core.Entities.Config
{
    public class OrderConfig : EntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.CustomerId).IsRequired();

            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,4)");

            builder.Property(x => x.Remark).HasMaxLength(OrdConsts.Remark_MaxLength);

            builder.OwnsOne(x => x.Status, y =>
            {
                y.Property(y => y.Code).IsRequired().HasColumnName("StatusCode");
                y.Property(y => y.ChangesReason).HasColumnName("StatusChangesReason").HasMaxLength(OrdConsts.ChangesReason_MaxLength);
            });


            builder.OwnsOne(x => x.Receiver, y =>
            {
                y.Property(y => y.Name).IsRequired().HasColumnName("ReceiverName").HasMaxLength(OrdConsts.Name_MaxLength);
                y.Property(y => y.Phone).IsRequired().HasColumnName("ReceiverPhone").HasMaxLength(OrdConsts.Phone_MaxLength);
                y.Property(y => y.Address).IsRequired().HasColumnName("ReceiverAddress").HasMaxLength(OrdConsts.Address_MaxLength);
            });

            builder.HasMany(x => x.Items).WithOne().HasForeignKey(y => y.OrderId);
        }
    }
}
