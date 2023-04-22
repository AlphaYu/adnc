namespace Adnc.Demo.Usr.Repository.Entities.Config;

public class UserConfig : AbstractEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Account).HasMaxLength(UserConsts.Account_MaxLength);
        builder.Property(x => x.Avatar).HasMaxLength(UserConsts.Avatar_MaxLength);
        builder.Property(x => x.Email).HasMaxLength(UserConsts.Email_Maxlength);
        builder.Property(x => x.Name).HasMaxLength(UserConsts.Name_Maxlength);
        builder.Property(x => x.Password).HasMaxLength(UserConsts.Password_Maxlength);
        builder.Property(x => x.Phone).HasMaxLength(UserConsts.Phone_Maxlength);
        builder.Property(x => x.RoleIds).HasMaxLength(UserConsts.RoleIds_Maxlength);
        builder.Property(x => x.Salt).HasMaxLength(UserConsts.Salt_Maxlength);

        builder.HasOne(d => d.Dept);
    }
}