using Adnc.Shared.Const.Entity.Cust;
using Adnc.Shared.Repository.EfEntities.Config;
using Adnc.UnitTest.TestCases.Repositories.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerFinanceConfig : AbstractEntityTypeConfiguration<CustomerFinance>
{
    public override void Configure(EntityTypeBuilder<CustomerFinance> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).IsRequired().HasMaxLength(CustomerFinanceConsts.Account_MaxLength);
        builder.Property(x => x.Balance).IsRequired().HasColumnType("decimal(18,4)");
    }
}