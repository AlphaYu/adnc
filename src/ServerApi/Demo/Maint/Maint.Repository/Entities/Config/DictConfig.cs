namespace Adnc.Demo.Maint.Repository.Entities.Config;

public class DictConfig : AbstractEntityTypeConfiguration<Dict>
{
    public override void Configure(EntityTypeBuilder<Dict> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Value).HasMaxLength(DictConsts.Value_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(DictConsts.Name_MaxLength);
    }
}