namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class DetpConfig : AbstractEntityTypeConfiguration<Organization>
{
    public override void Configure(EntityTypeBuilder<Organization> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(Organization.Name_MaxLength);
        builder.Property(x => x.Code).HasMaxLength(Organization.Code_MaxLength);
        builder.Property(x => x.ParentIds).HasMaxLength(Organization.Pids_MaxLength);
    }
}
