namespace Adnc.Shared.Remote.Http.Messages;

public class LoginResponse
{
    /// <summary>
    /// Constructor
    /// Added by garfield on 20220530 to fix the warning
    /// </summary>
    public LoginResponse(string token, string refreshToken) => (Token, RefreshToken) = (token, refreshToken);

    public string Token { get; set; }

    public string RefreshToken { get; set; }
}
