using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 刷新Token实体
    /// </summary>
    public class UserRefreshTokenDto : IInputDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }

    }
}
