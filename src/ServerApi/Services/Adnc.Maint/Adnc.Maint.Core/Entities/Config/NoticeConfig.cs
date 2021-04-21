using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;

namespace Adnc.Maint.Core.Entities.Config
{
    public class NoticeConfig : EntityTypeConfiguration<SysNotice>
    {
        public override void Configure(EntityTypeBuilder<SysNotice> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Content).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Title).HasMaxLength(64);
        }
    }
}
