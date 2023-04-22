namespace Adnc.Demo.Maint.Repository.Entities.Config;

public class CfgConfig : AbstractEntityTypeConfiguration<Cfg>
{
    public override void Configure(EntityTypeBuilder<Cfg> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(CfgConsts.Name_MaxLength);
        builder.Property(x => x.Value).HasMaxLength(CfgConsts.Value_MaxLength);
        builder.Property(x => x.Description).HasMaxLength(CfgConsts.Description_MaxLength);
    }
}