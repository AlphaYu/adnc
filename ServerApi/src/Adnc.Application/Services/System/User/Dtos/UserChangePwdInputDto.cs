using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 修改密码Model
    /// </summary>
    public class UserChangePwdInputDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// 当前密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        public string RePassword { get; set; }
    }
}
