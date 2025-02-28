using Adnc.Demo.Shared.Const.Entity.Cust;

namespace Adnc.Demo.Cust.Api.Repository.Entities.Config;

public class CustomerFinanceConfig : AbstractEntityTypeConfiguration<CustomerFinance>
{
    public override void Configure(EntityTypeBuilder<CustomerFinance> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).IsRequired().HasMaxLength(CustomerFinanceConsts.Account_MaxLength);
        builder.Property(x => x.Balance).IsRequired().HasColumnType("decimal(18,4)");
    }
}