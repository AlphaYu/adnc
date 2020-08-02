using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Core.Entities.Config
{
    public class RoleConfig : IEntityTypeConfiguration<SysRole>
    {
        public void Configure(EntityTypeBuilder<SysRole> builder)
        {
            //查询过滤器 Query Filter
            //builder.HasQueryFilter(e => !e.IsDeleted);
            //一对多,SysDept没有UserId字段
            builder.HasMany(d => d.Relations)
                .WithOne(p => p.Role)
                .HasForeignKey(d => d.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
