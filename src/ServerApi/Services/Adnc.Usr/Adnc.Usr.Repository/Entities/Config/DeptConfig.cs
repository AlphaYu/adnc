using Adnc.Infra.Entities.Config;
using Adnc.Shared.Consts.Entity.Usr;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Entities.Config
{
    public class DetpConfig : EntityTypeConfiguration<SysDept>
    {
        public override void Configure(EntityTypeBuilder<SysDept> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(DeptConsts.FullName_MaxLength);
            builder.Property(x => x.SimpleName).IsRequired().HasMaxLength(DeptConsts.SimpleName_MaxLength);
            builder.Property(x => x.Tips).HasMaxLength(DeptConsts.Tips_MaxLength);
            builder.Property(x => x.Pids).HasMaxLength(DeptConsts.Pids_MaxLength);
        }
    }
}