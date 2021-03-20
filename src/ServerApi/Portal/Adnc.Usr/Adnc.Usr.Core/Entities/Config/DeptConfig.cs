using Adnc.Core.Shared.Entities.Config;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Core.Entities.Config
{
    public class DetpConfig : EntityTypeConfiguration<SysDept>
    {
        public override void Configure(EntityTypeBuilder<SysDept> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(32);
            builder.Property(x => x.SimpleName).IsRequired().HasMaxLength(16);
            builder.Property(x => x.Tips).HasMaxLength(64);
            builder.Property(x => x.Pids).HasMaxLength(80);
        }
    }
}
