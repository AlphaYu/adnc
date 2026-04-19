namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class RoleConfig : AbstractEntityTypeConfiguration<Role>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(Role.Name_MaxLength);
        builder.Property(x => x.Code).HasMaxLength(Role.Code_MaxLength);
    }
}
