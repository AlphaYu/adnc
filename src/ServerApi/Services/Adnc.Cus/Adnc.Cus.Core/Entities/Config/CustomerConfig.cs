using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Cust;

namespace Adnc.Cus.Core.Entities.Config
{
    public class CustomerConfig : EntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.HasOne(d => d.FinanceInfo).WithOne(p => p.Customer).HasForeignKey<CustomerFinance>(d => d.Id).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.TransactionLogs).WithOne().HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(CustConsts.Account_MaxLength);

            builder.Property(x => x.Nickname).IsRequired().HasMaxLength(CustConsts.Nickname_MaxLength);

            builder.Property(x => x.Realname).IsRequired().HasMaxLength(CustConsts.Realname_Maxlength);
        }
    }
}
