using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.Models
{
    public class JwtAccountlistModel
    {
        public string AccessToken{get;set;}
        public string RefreshToken { get; set; }
        public DateTime Expire { get; set; }
        /// <summary>
        /// 1 白名单 2 黑名单
        /// </summary>
        public int Status { get; set; }
    }
}
