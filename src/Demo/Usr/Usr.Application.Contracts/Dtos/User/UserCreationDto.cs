namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    public class UserCreationDto : UserCreationAndUpdationDto
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