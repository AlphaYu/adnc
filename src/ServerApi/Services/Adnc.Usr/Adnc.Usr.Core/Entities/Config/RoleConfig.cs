using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Usr;

namespace Adnc.Usr.Core.Entities.Config
{
    public class RoleConfig : EntityTypeConfiguration<SysRole>
    {
        public override void Configure(EntityTypeBuilder<SysRole> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(RoleConsts.Name_MaxLength);
            builder.Property(x => x.Tips).HasMaxLength(RoleConsts.Tips_MaxLength);

            //一对多,SysDept没有UserId字段
            builder.HasMany(d => d.Relations)
                   .WithOne(p => p.Role)
                   .HasForeignKey(d => d.RoleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
