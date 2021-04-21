using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class UserLoginDto : IInputDto
    {
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

    }
}
