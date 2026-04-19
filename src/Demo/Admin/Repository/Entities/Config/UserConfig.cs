namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class UserConfig : AbstractEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).HasMaxLength(User.Account_MaxLength);
        builder.Property(x => x.Avatar).HasMaxLength(User.Avatar_MaxLength);
        builder.Property(x => x.Email).HasMaxLength(User.Email_Maxlength);
        builder.Property(x => x.Name).HasMaxLength(User.Name_Maxlength);
        builder.Property(x => x.Password).HasMaxLength(User.Password_Maxlength);
        builder.Property(x => x.Mobile).HasMaxLength(User.Phone_Maxlength);
        builder.Property(x => x.Salt).HasMaxLength(User.Salt_Maxlength);
    }
}
