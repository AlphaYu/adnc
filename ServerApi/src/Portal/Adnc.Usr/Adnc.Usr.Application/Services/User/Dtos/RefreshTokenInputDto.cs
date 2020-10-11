using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    /// <summary>
    /// 刷新Token实体
    /// </summary>
    public class RefreshTokenInputDto:BaseDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }

    }
}
