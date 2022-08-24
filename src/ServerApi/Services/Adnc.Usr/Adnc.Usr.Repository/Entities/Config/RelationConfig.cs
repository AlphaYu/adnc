namespace Adnc.Usr.Repository.Entities.Config;

public class RelationConfig : AbstractEntityTypeConfiguration<SysRelation>
{
    public override void Configure(EntityTypeBuilder<SysRelation> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.MenuId).IsRequired();
    }
}