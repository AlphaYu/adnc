using Adnc.Demo.Const.Entity.Cust;

namespace Adnc.Demo.Cust.Api.Repository.Entities.Config;

public class TransactionLogConfig : AbstractEntityTypeConfiguration<TransactionLog>
{
    public override void Configure(EntityTypeBuilder<TransactionLog> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).HasMaxLength(CustomerTransactionLogConsts.Account_MaxLength);
        builder.Property(x => x.Amount).HasColumnType("decimal(18,4)");
        builder.Property(x => x.ChangingAmount).HasColumnType("decimal(18,4)");
        builder.Property(x => x.ChangedAmount).HasColumnType("decimal(18,4)");
        builder.Property(x => x.Remark).HasMaxLength(CustomerTransactionLogConsts.Remark_MaxLength);
    }
}
