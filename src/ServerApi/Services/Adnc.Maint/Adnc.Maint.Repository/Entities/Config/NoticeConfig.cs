namespace Adnc.Maint.Entities.Config;

public class NoticeConfig : AbstractEntityTypeConfiguration<SysNotice>
{
    public override void Configure(EntityTypeBuilder<SysNotice> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Content).IsRequired().HasMaxLength(NoticeConsts.Content_MaxLength);
        builder.Property(x => x.Title).HasMaxLength(NoticeConsts.Title_MaxLength);
    }
}