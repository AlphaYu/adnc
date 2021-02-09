using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class UserTokenInfoDto : IDto
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
