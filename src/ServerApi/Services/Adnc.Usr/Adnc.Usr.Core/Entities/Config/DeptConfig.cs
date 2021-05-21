using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Usr;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Core.Entities.Config
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