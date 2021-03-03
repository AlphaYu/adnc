using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;

namespace Adnc.Cus.Core.Entities.Config
{
    public class CustomerConfig : EntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.HasOne(d => d.FinanceInfo).WithOne(p => p.Customer).HasForeignKey<CustomerFinance>(d => d.Id).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.TransactionLogs).WithOne().HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(16);

            builder.Property(x => x.Nickname).IsRequired().HasMaxLength(16);

            builder.Property(x => x.Realname).IsRequired().HasMaxLength(16);
        }
    }
}
