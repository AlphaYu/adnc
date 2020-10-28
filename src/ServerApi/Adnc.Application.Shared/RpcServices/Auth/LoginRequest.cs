using System.ComponentModel.DataAnnotations;

namespace Adnc.Application.Shared.RpcServices
{
    public class LoginRequest
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
