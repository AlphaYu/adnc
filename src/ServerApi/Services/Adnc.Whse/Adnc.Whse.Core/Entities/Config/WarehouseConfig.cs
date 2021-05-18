using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Whse;

namespace Adnc.Whse.Core.Entities.Config
{
    public class WarehouseConfig : EntityTypeConfiguration<Warehouse>
    {
        public override void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.BlockedQty).IsRequired();
            builder.Property(x => x.Qty).IsRequired();

            builder.OwnsOne(x => x.Position, y =>
            {
                y.Property(x => x.Code).IsRequired().HasColumnName("PositionCode").HasMaxLength(WhseConsts.Code_MaxLength);
                y.Property(x => x.Description).HasColumnName("PositionDescription").HasMaxLength(WhseConsts.Description_MaxLength);
            });
        }
    }
}
