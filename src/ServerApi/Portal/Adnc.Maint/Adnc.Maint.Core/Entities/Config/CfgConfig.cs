using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Maint.Core.Entities.Config
{
    public class CfgConfig : IEntityTypeConfiguration<SysCfg>
    {
        public void Configure(EntityTypeBuilder<SysCfg> builder)
        {
            //builder.Property<bool>("IsDeleted")
            //    .HasDefaultValue(false);
            //builder.HasQueryFilter(d => EF.Property<bool>(d, "IsDeleted") == false);
            builder.Property(d => d.IsDeleted)
                   .HasDefaultValue(false);
            builder.HasQueryFilter(d => d.IsDeleted == false);
        }
    }
}
