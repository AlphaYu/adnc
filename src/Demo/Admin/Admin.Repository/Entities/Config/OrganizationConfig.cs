namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class DetpConfig : AbstractEntityTypeConfiguration<Organization>
{
    public override void Configure(EntityTypeBuilder<Organization> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(DeptConsts.Name_MaxLength);
        builder.Property(x => x.Code).HasMaxLength(DeptConsts.Code_MaxLength);
        builder.Property(x => x.ParentIds).HasMaxLength(DeptConsts.Pids_MaxLength);
    }
}
