namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the access token and refresh token information returned after authentication.
/// </summary>
public record UserTokenInfoDto : IDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTokenInfoDto"/> record.
    /// </summary>
    /// <param name="token">The access token.</param>
    /// <param name="exprie">The access token expiration time.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="refreshExprie">The refresh token expiration time.</param>
    public UserTokenInfoDto(string token, DateTime exprie, string refreshToken, DateTime refreshExprie)
    {
        Token = token;
        Expire = exprie;
        RefreshToken = refreshToken;
        RefreshExpire = refreshExprie;
    }

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets the access token expiration time.
    /// </summary>
    public DateTime Expire { get; set; }

    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Gets or sets the refresh token expiration time.
    /// </summary>
    public DateTime RefreshExpire { get; set; }
}
