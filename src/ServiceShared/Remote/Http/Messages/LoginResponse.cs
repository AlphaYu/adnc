namespace Adnc.Shared.Remote.Http.Messages;

public class LoginResponse
{
    /// <summary>
    /// 构造函数
    /// 修复Warning, add by garfield 20220530
    /// </summary>
    public LoginResponse(string token, string refreshToken) => (Token, RefreshToken) = (token, refreshToken);

    public string Token { get; set; }

    public string RefreshToken { get; set; }
}
