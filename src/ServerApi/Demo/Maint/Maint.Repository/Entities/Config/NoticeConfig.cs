namespace Adnc.Demo.Maint.Repository.Entities.Config;

public class NoticeConfig : AbstractEntityTypeConfiguration<Notice>
{
    public override void Configure(EntityTypeBuilder<Notice> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Content).HasMaxLength(NoticeConsts.Content_MaxLength);
        builder.Property(x => x.Title).HasMaxLength(NoticeConsts.Title_MaxLength);
    }
}