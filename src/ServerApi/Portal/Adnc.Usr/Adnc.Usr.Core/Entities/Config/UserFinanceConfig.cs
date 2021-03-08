using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Core.Entities.Config
{
    public class UserFinanceConfig : IEntityTypeConfiguration<SysUserFinance>
    {
        public void Configure(EntityTypeBuilder<SysUserFinance> builder)
        {
            builder.Property(e => e.Amount).HasColumnType("decimal(18,4)");

            builder.Property(e => e.RowVersion)
                .HasColumnType("timestamp(3)")
                .HasDefaultValueSql("'2000-07-01 22:33:02.559'") //默认值
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
