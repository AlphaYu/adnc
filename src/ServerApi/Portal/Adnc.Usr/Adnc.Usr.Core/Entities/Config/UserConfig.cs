using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Core.Entities.Config
{
    public class UserConfig : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            //查询过滤器 Query Filter
            builder.Property(d => d.IsDeleted)
                   .HasDefaultValue(false);
            builder.HasQueryFilter(d => d.IsDeleted == false);

            builder.HasOne(d => d.UserFinance)
                   .WithOne(p => p.User)
                   .HasForeignKey<SysUserFinance>(p=>p.Id)
                   .OnDelete(DeleteBehavior.Cascade);

            //一对多,SysDept没有UserId字段
            builder.HasOne(d => d.Dept)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.DeptId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
