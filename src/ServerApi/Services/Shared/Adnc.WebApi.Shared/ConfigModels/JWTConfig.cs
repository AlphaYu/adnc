namespace Adnc.WebApi.Shared
{
    /// <summary>
    /// JWT配置
    /// </summary>
    public class JWTConfig
    {
        /// <summary>
        /// 加密Key
        /// </summary>
        public string SymmetricSecurityKey { get; set; }

        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 时间歪斜
        /// </summary>
        public int ClockSkew { get; set; }

        /// <summary>
        /// Accessoken受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// RefreshToken受众
        /// </summary>
        public string RefreshTokenAudience { get; set; }

        /// <summary>
        /// AccessToken过期时间，单位分钟
        /// </summary>
        public int Expire { get; set; }

        /// <summary>
        /// RefreshToken过期时间，单位分钟
        /// </summary>
        public int RefreshTokenExpire { get; set; }
    }
}