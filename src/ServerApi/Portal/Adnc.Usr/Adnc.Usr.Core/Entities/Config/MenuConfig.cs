using Adnc.Core.Shared.Entities.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Core.Entities.Config
{
    public class MenuConfig : EntityTypeConfiguration<SysMenu>
    {
        public override void Configure(EntityTypeBuilder<SysMenu> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(16);
            builder.Property(x => x.PCode).HasMaxLength(16);
            builder.Property(x => x.PCodes).HasMaxLength(128);
            builder.Property(x => x.Component).HasMaxLength(64);
            builder.Property(x => x.Icon).HasMaxLength(16);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(16);
            builder.Property(x => x.Tips).HasMaxLength(32);
            builder.Property(x => x.Url).HasMaxLength(64);

            builder.HasMany(d => d.Relations)
                   .WithOne(m => m.Menu)
                   .HasForeignKey(d => d.MenuId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
