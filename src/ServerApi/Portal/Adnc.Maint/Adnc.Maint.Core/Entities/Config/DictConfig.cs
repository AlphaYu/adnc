using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;

namespace Adnc.Maint.Core.Entities.Config
{
    public class DictConfig : EntityTypeConfiguration<SysDict>
    {
        public override void Configure(EntityTypeBuilder<SysDict> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Value).HasMaxLength(16);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Pid).IsRequired();
        }
    }
}
