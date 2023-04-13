namespace Adnc.Demo.Usr.Repository.Entities.Config;

public class DetpConfig : AbstractEntityTypeConfiguration<Organization>
{
    public override void Configure(EntityTypeBuilder<Organization> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.FullName).HasMaxLength(DeptConsts.FullName_MaxLength);
        builder.Property(x => x.SimpleName).HasMaxLength(DeptConsts.SimpleName_MaxLength);
        builder.Property(x => x.Tips).HasMaxLength(DeptConsts.Tips_MaxLength);
        builder.Property(x => x.Pids).HasMaxLength(DeptConsts.Pids_MaxLength);
    }
}