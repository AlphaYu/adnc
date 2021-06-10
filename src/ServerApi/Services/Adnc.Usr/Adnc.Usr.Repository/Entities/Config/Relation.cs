using Adnc.Infra.Entities.Config;
using Adnc.Usr.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Repository.Entities.Config
{
    public class RelationConfig : EntityTypeConfiguration<SysRelation>
    {
        public override void Configure(EntityTypeBuilder<SysRelation> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.RoleId).IsRequired();
            builder.Property(x => x.MenuId).IsRequired();
        }
    }
}