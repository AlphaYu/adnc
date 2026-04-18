namespace Adnc.Infra.Unittest.Reposity.Fixtures.Entities.Config;
/// <summary>
/// Customer
/// </summary>
public static class CustomerConsts
{
    public const int Account_MaxLength = 16;
    public const int Password_Maxlength = 32;
    public const int Nickname_MaxLength = 16;
    public const int Realname_Maxlength = 16;
}

/// <summary>
/// Customer finance table
/// </summary>
public static class CustomerFinanceConsts
{
    public const int Account_MaxLength = 16;
}

/// <summary>
/// Customer finance change
/// </summary>
public static class CustomerTransactionLogConsts
{
    public const int Account_MaxLength = 16;
    public const int Remark_MaxLength = 64;
}

/// <summary>
/// Project table
/// </summary>
public static class ProjectConsts
{
    public const int Name_MaxLength = 50;
}
