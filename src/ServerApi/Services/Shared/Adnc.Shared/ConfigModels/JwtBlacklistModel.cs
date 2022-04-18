using System;

namespace Adnc.Shared.ConfigModels
{
    public class JwtAccountlistModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expire { get; set; }

        /// <summary>
        /// 1 白名单 2 黑名单
        /// </summary>
        public int Status { get; set; }
    }
}