using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;

namespace Adnc.Usr.Core.Entities.Config
{
    public class UserConfig : EntityTypeConfiguration<SysUser>
    {
        public override void Configure(EntityTypeBuilder<SysUser> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(16);
            builder.Property(x => x.Avatar).HasMaxLength(64);
            builder.Property(x => x.Email).HasMaxLength(32);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(16);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(32);
            builder.Property(x => x.Phone).HasMaxLength(11);
            builder.Property(x => x.RoleIds).HasMaxLength(72);
            builder.Property(x => x.Salt).IsRequired().HasMaxLength(6);

            //一对多,SysDept没有UserId字段
            builder.HasOne(d => d.Dept)
                   .WithMany(p => p.Users)
                   .HasForeignKey(d => d.DeptId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
