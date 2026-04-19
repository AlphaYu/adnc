namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class SysConfigConfig : AbstractEntityTypeConfiguration<SysConfig>
{
    public override void Configure(EntityTypeBuilder<SysConfig> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Key).HasMaxLength(SysConfig.Key_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(SysConfig.Name_MaxLength);
        builder.Property(x => x.Value).HasMaxLength(SysConfig.Value_MaxLength);
        builder.Property(x => x.Remark).HasMaxLength(SysConfig.Remark_MaxLength);
    }
}
