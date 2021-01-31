using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Warehouse.Core.Entities.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //builder.HasOne(d => d.CusFinance)
            //    .WithOne(p => p.Customer)
            //    .HasForeignKey<CusFinance>(d => d.ID)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
