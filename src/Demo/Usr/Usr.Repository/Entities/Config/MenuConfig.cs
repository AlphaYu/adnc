namespace Adnc.Demo.Usr.Repository.Entities.Config;

public class MenuConfig : AbstractEntityTypeConfiguration<Menu>
{
    public override void Configure(EntityTypeBuilder<Menu> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Code).HasMaxLength(MenuConsts.Code_MaxLength);
        builder.Property(x => x.PCode).HasMaxLength(MenuConsts.PCode_MaxLength);
        builder.Property(x => x.PCodes).HasMaxLength(MenuConsts.PCodes_MaxLength);
        builder.Property(x => x.Component).HasMaxLength(MenuConsts.Component_MaxLength);
        builder.Property(x => x.Icon).HasMaxLength(MenuConsts.Icon_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(MenuConsts.Name_MaxLength);
        builder.Property(x => x.Tips).HasMaxLength(MenuConsts.Tips_MaxLength);
        builder.Property(x => x.Url).HasMaxLength(MenuConsts.Url_MaxLength);
    }
}