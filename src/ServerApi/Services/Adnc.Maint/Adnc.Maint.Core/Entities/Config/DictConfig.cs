using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Maint;

namespace Adnc.Maint.Core.Entities.Config
{
    public class DictConfig : EntityTypeConfiguration<SysDict>
    {
        public override void Configure(EntityTypeBuilder<SysDict> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Value).HasMaxLength(DictConsts.Value_MaxLength);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(DictConsts.Name_MaxLength);
            builder.Property(x => x.Pid).IsRequired();
        }
    }
}
