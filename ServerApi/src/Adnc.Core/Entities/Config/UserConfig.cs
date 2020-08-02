using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Core.Entities.Config
{
    public class UserConfig : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            //查询过滤器 Query Filter
            //builder.HasQueryFilter(e => !e.IsDeleted);
            //一对多,SysDept没有UserId字段
            builder.HasOne(d => d.Dept)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.DeptId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
