using Adnc.Core.Shared.Entities.Config;
using Adnc.Core.Shared.EntityConsts.Cust;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Cus.Core.Entities.Config
{
    public class CustomerFinanceConfig : EntityTypeConfiguration<CustomerFinance>
    {
        public override void Configure(EntityTypeBuilder<CustomerFinance> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(CustomerFinanceConsts.Account_MaxLength);
            builder.Property(x => x.Balance).IsRequired().HasColumnType("decimal(18,4)");
        }
    }
}