namespace Adnc.Demo.Ord.Domain.EntityConfig;

public class OrderConfig : AbstractEntityTypeConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CustomerId);

        builder.Property(x => x.Amount).HasColumnType("decimal(18,4)");

        builder.Property(x => x.Remark).HasMaxLength(Order.Remark_MaxLength);

        builder.OwnsOne(x => x.Status, y =>
        {
            y.Property(z => z.Code).HasColumnName("statuscode");
            y.Property(z => z.ChangesReason).HasColumnName("statuschangesreason").HasMaxLength(Order.ChangesReason_MaxLength);
        });

        builder.OwnsOne(x => x.Receiver, y =>
        {
            y.Property(z => z.Name).HasColumnName("receivername").HasMaxLength(Order.Name_MaxLength);
            y.Property(z => z.Phone).HasColumnName("receiverphone").HasMaxLength(Order.Phone_MaxLength);
            y.Property(z => z.Address).HasColumnName("receiveraddress").HasMaxLength(Order.Address_MaxLength);
        });

        builder.HasMany(x => x.Items).WithOne().HasForeignKey(y => y.OrderId);
    }
}
