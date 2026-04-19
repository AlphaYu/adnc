namespace Adnc.Demo.Cust.Api.Repository.Entities.Config;

public class CustomerConfig : AbstractEntityTypeConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);

        builder.HasOne(d => d.FinanceInfo).WithOne().HasForeignKey<Finance>(d => d.Id).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(d => d.TransactionLogs).WithOne().HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Account).HasMaxLength(Customer.Account_MaxLength);
        builder.Property(x => x.Password).HasMaxLength(Customer.Password_Maxlength);
        builder.Property(x => x.Nickname).HasMaxLength(Customer.Nickname_MaxLength);
        builder.Property(x => x.Realname).HasMaxLength(Customer.Realname_Maxlength);
    }
}
