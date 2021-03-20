using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;

namespace Adnc.Maint.Core.Entities.Config
{
    public class CfgConfig : EntityTypeConfiguration<SysCfg>
    {
        public override void Configure(EntityTypeBuilder<SysCfg> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Value).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).HasMaxLength(256);
        }
    }
}
