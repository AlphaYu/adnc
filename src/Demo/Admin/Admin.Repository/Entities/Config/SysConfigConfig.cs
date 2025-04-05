namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class SysConfigConfig : AbstractEntityTypeConfiguration<SysConfig>
{
    public override void Configure(EntityTypeBuilder<SysConfig> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Key).HasMaxLength(SysConfigConsts.Key_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(SysConfigConsts.Name_MaxLength);
        builder.Property(x => x.Value).HasMaxLength(SysConfigConsts.Value_MaxLength);
        builder.Property(x => x.Remark).HasMaxLength(SysConfigConsts.Remark_MaxLength);
    }
}
