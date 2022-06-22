namespace Adnc.Usr.Application.Contracts.Dtos
{
    /// <summary>
    /// 刷新Token实体
    /// </summary>
    public class UserRefreshTokenDto : IInputDto
    {
        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
    }
}