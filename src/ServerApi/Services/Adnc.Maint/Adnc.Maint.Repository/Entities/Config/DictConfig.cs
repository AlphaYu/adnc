namespace Adnc.Maint.Entities.Config;

public class DictConfig : AbstractEntityTypeConfiguration<SysDict>
{
    public override void Configure(EntityTypeBuilder<SysDict> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Value).HasMaxLength(DictConsts.Value_MaxLength);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(DictConsts.Name_MaxLength);
        builder.Property(x => x.Pid).IsRequired();
    }
}