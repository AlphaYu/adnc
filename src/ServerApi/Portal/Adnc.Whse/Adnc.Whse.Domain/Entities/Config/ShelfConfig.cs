using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities;

namespace Adnc.Whse.Domain.Entities.Config
{
    public class ShelfConfig : EntityTypeConfiguration<Shelf>
    {
        public override void Configure(EntityTypeBuilder<Shelf> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FreezedQty).IsRequired();
            builder.Property(x => x.Qty).IsRequired();

            builder.OwnsOne(x => x.Position, y =>
            {
                y.Property(x => x.Code).IsRequired().HasColumnName("PositionCode").HasMaxLength(32);
                y.Property(x => x.Description).HasColumnName("PositionDescription").HasMaxLength(64);
            });
        }
    }
}
