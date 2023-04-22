namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 刷新Token实体
    /// </summary>
    public class UserRefreshTokenDto : InputDto
    {
        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
    }
}