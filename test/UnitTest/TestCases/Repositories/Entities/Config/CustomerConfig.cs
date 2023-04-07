using Adnc.Shared.Const.Entity.Cust;
using Adnc.Shared.Repository.EfEntities.Config;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.UnitTest.TestCases.Repositories.Entities.Config;

public class CustomerConfig : AbstractEntityTypeConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);

        builder.HasOne(d => d.FinanceInfo).WithOne(p => p.Customer).HasForeignKey<CustomerFinance>(d => d.Id).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(d => d.TransactionLogs).WithOne().HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Account).IsRequired().HasMaxLength(CustomerConsts.Account_MaxLength);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(CustomerConsts.Password_Maxlength);
        builder.Property(x => x.Nickname).IsRequired().HasMaxLength(CustomerConsts.Nickname_MaxLength);
        builder.Property(x => x.Realname).IsRequired().HasMaxLength(CustomerConsts.Realname_Maxlength);
    }
}