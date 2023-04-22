namespace Adnc.Demo.Usr.Repository.Entities.Config;

public class RoleConfig : AbstractEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(RoleConsts.Name_MaxLength);
        builder.Property(x => x.Tips).HasMaxLength(RoleConsts.Tips_MaxLength);
    }
}