namespace Adnc.Shared.Rpc.Http.Rtos;

public class LoginInputRto
{
    /// <summary>
    /// 构造函数
    /// 修复Warning, add by garfield 20220530
    /// </summary>
    public LoginInputRto(string account, string password) => (Account, Password) = (account, password);

    /// <summary>
    /// 账户
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}