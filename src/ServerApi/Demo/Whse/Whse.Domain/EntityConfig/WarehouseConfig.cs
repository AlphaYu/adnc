using Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

namespace Adnc.Demo.Whse.Domain.EntityConfig;

public class WarehouseConfig : AbstractEntityTypeConfiguration<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.BlockedQty).IsRequired();
        builder.Property(x => x.Qty).IsRequired();

        builder.OwnsOne(x => x.Position, y =>
        {
            y.Property(x => x.Code).IsRequired().HasColumnName("positioncode").HasMaxLength(WhseConsts.Code_MaxLength);
            y.Property(x => x.Description).HasColumnName("positiondescription").HasMaxLength(WhseConsts.Description_MaxLength);
        });
    }
}