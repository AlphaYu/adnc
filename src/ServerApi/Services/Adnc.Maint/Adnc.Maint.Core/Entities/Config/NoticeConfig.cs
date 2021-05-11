using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Maint;

namespace Adnc.Maint.Core.Entities.Config
{
    public class NoticeConfig : EntityTypeConfiguration<SysNotice>
    {
        public override void Configure(EntityTypeBuilder<SysNotice> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Content).IsRequired().HasMaxLength(NoticeConsts.Content_MaxLength);
            builder.Property(x => x.Title).HasMaxLength(NoticeConsts.Title_MaxLength);
        }
    }
}
