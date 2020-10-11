using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Usr.Core.Entities.Config
{
    public class MenuConfig : IEntityTypeConfiguration<SysMenu>
    {
        public void Configure(EntityTypeBuilder<SysMenu> builder)
        {

            builder.HasMany(d=>d.Relations)
                .WithOne(m=>m.Menu)
                .HasForeignKey(d => d.MenuId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
