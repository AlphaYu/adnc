using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Cus.Core.Entities.Config
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(d => d.CusFinance)
                .WithOne(p => p.Customer)
                .HasForeignKey<CusFinance>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
