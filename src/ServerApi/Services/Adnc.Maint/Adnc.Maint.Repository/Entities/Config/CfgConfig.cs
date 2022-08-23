namespace Adnc.Maint.Entities.Config;

public class CfgConfig : AbstractEntityTypeConfiguration<SysCfg>
{
    public override void Configure(EntityTypeBuilder<SysCfg> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(CfgConsts.Name_MaxLength);
        builder.Property(x => x.Value).IsRequired().HasMaxLength(CfgConsts.Value_MaxLength);
        builder.Property(x => x.Description).HasMaxLength(CfgConsts.Description_MaxLength);
    }
}