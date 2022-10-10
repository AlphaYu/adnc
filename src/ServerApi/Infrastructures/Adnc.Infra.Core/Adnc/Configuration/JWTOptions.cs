namespace Adnc.Infra.Core.Configuration;

/// <summary>
/// JWT配置
/// </summary>
public class JWTOptions
{
    public Encoding Encoding => Encoding.UTF8;

    /// <summary>
    /// 是否校验颁发者
    /// </summary>
    public bool ValidateIssuer { get; set; } = default!;

    /// <summary>
    /// 颁发者
    /// </summary>
    public string ValidIssuer { get; set; } = string.Empty;

    /// <summary>
    /// 是否校验签名
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; } = default!;

    /// <summary>
    /// 签名
    /// </summary>
    public string SymmetricSecurityKey { get; set; } = string.Empty;
    public string IssuerSigningKey { get; set; } = string.Empty;

    /// <summary>
    /// 是否校验受众
    /// </summary>
    public bool ValidateAudience { get; set; } = default!;

    /// <summary>
    /// Accessoken受众
    /// </summary>
    public string ValidAudience { get; set; } = string.Empty;

    /// <summary>
    /// RefreshToken受众
    /// </summary>
    public string RefreshTokenAudience { get; set; } = string.Empty;

    /// <summary>
    /// 校验Lifetime
    /// </summary>
    public bool ValidateLifetime { get; set; } = default!;

    /// <summary>
    ///  校验是否有Expire字段
    /// </summary>
    public bool RequireExpirationTime { get; set; }

    /// <summary>
    /// 时间歪斜，单位秒
    /// </summary>
    public int ClockSkew { get; set; } = default;

    /// <summary>
    /// AccessToken过期时间，单位分钟
    /// </summary>
    public int Expire { get; set; } = default;

    /// <summary>
    /// RefreshToken过期时间，单位分钟
    /// </summary>
    public int RefreshTokenExpire { get; set; } = default;

}