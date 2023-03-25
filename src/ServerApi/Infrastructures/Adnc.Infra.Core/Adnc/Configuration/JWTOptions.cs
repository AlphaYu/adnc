namespace Adnc.Infra.Core.Configuration;

/// <summary>
/// JWT configuration
/// </summary>
public class JWTOptions
{
    /// <summary>
    /// Encoding for JWT
    /// </summary>
    public Encoding Encoding => Encoding.UTF8;

    /// <summary>
    /// Whether to validate the issuer
    /// </summary>
    public bool ValidateIssuer { get; set; } = default;

    /// <summary>
    /// The issuer of the JWT
    /// </summary>
    public string ValidIssuer { get; set; } = string.Empty;

    /// <summary>
    /// Whether to validate the signature
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; } = default;

    /// <summary>
    /// The symmetric security key used to sign the JWT
    /// </summary>
    public string SymmetricSecurityKey { get; set; } = string.Empty;

    /// <summary>
    /// The issuer signing key used to sign the JWT
    /// </summary>
    public string IssuerSigningKey { get; set; } = string.Empty;

    /// <summary>
    /// Whether to validate the audience
    /// </summary>
    public bool ValidateAudience { get; set; } = default!;

    /// <summary>
    /// The audience for the access token
    /// </summary>
    public string ValidAudience { get; set; } = string.Empty;

    /// <summary>
    /// The audience for the refresh token
    /// </summary>
    public string RefreshTokenAudience { get; set; } = string.Empty;

    /// <summary>
    /// Whether to validate the lifetime
    /// </summary>
    public bool ValidateLifetime { get; set; } = default!;

    /// <summary>
    /// Whether to require the expiration time
    /// </summary>
    public bool RequireExpirationTime { get; set; }

    /// <summary>
    /// Clock skew in seconds
    /// </summary>
    public int ClockSkew { get; set; } = default;

    /// <summary>
    /// Expiration time for access token in minutes
    /// </summary>
    public int Expire { get; set; } = default;

    /// <summary>
    /// Expiration time for refresh token in minutes
    /// </summary>
    public int RefreshTokenExpire { get; set; } = default;
}