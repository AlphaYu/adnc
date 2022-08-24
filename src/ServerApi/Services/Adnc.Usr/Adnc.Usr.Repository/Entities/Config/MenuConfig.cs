namespace Adnc.Usr.Repository.Entities.Config;

public class MenuConfig : AbstractEntityTypeConfiguration<SysMenu>
{
    public override void Configure(EntityTypeBuilder<SysMenu> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(MenuConsts.Code_MaxLength);
        builder.Property(x => x.PCode).HasMaxLength(MenuConsts.PCode_MaxLength);
        builder.Property(x => x.PCodes).HasMaxLength(MenuConsts.PCodes_MaxLength);
        builder.Property(x => x.Component).HasMaxLength(MenuConsts.Component_MaxLength);
        builder.Property(x => x.Icon).HasMaxLength(MenuConsts.Icon_MaxLength);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(MenuConsts.Name_MaxLength);
        builder.Property(x => x.Tips).HasMaxLength(MenuConsts.Tips_MaxLength);
        builder.Property(x => x.Url).HasMaxLength(MenuConsts.Url_MaxLength);

        builder.HasMany(d => d.Relations)
               .WithOne(m => m.Menu)
               .HasForeignKey(d => d.MenuId)
               .IsRequired()
               .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
    }
}