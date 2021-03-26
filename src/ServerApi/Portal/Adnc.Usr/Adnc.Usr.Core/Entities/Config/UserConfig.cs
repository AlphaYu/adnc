using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Usr.Core.Entities.Consts;

namespace Adnc.Usr.Core.Entities.Config
{
    public class UserConfig : EntityTypeConfiguration<SysUser>
    {
        public override void Configure(EntityTypeBuilder<SysUser> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(UserConsts.Account_MaxLength);
            builder.Property(x => x.Avatar).HasMaxLength(UserConsts.Avatar_MaxLength);
            builder.Property(x => x.Email).HasMaxLength(UserConsts.Email_Maxlength);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(UserConsts.Name_Maxlength);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(UserConsts.Password_Maxlength);
            builder.Property(x => x.Phone).HasMaxLength(UserConsts.Phone_Maxlength);
            builder.Property(x => x.RoleIds).HasMaxLength(UserConsts.RoleIds_Maxlength);
            builder.Property(x => x.Salt).IsRequired().HasMaxLength(UserConsts.Salt_Maxlength);

            //一对多,SysDept没有UserId字段
            builder.HasOne(d => d.Dept)
                   .WithMany(p => p.Users)
                   .HasForeignKey(d => d.DeptId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
