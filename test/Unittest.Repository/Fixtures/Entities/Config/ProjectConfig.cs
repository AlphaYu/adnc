namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities.Config;

public class ProjectConfig : AbstractEntityTypeConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(ProjectConsts.Name_MaxLength);
    }
}
