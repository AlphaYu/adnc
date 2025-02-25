using Adnc.Demo.Shared.Const.Entity.Cust;

namespace Adnc.Demo.Cust.Api.Repository.Entities.Config;

public class CustomerTransactionLogConfig : AbstractEntityTypeConfiguration<CustomerTransactionLog>
{
    public override void Configure(EntityTypeBuilder<CustomerTransactionLog> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).IsRequired().HasMaxLength(CustomerTransactionLogConsts.Account_MaxLength);
        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,4)");
        builder.Property(x => x.ChangingAmount).IsRequired().HasColumnType("decimal(18,4)");
        builder.Property(x => x.ChangedAmount).IsRequired().HasColumnType("decimal(18,4)");
        builder.Property(x => x.Remark).HasMaxLength(CustomerTransactionLogConsts.Remark_MaxLength);
    }
}