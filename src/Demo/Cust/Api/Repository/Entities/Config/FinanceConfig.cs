using Adnc.Demo.Const.Entity.Cust;

namespace Adnc.Demo.Cust.Api.Repository.Entities.Config;

public class FinanceConfig : AbstractEntityTypeConfiguration<Finance>
{
    public override void Configure(EntityTypeBuilder<Finance> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).HasMaxLength(CustomerFinanceConsts.Account_MaxLength);
        builder.Property(x => x.Balance).HasColumnType("decimal(18,4)");
    }
}
