using System.ComponentModel.DataAnnotations;

namespace Adnc.Application.Shared.RpcServices.Rtos
{
    public class LoginRto
    {
        /// <summary>
        /// 账户
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
