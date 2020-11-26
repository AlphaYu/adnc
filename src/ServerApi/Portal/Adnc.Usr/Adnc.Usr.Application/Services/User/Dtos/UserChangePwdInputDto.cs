using Adnc.Application.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Adnc.Usr.Application.Dtos
{
    /// <summary>
    /// 修改密码Model
    /// </summary>
    public class UserChangePwdInputDto : BaseDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 当前密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        public string RePassword { get; set; }
    }
}
