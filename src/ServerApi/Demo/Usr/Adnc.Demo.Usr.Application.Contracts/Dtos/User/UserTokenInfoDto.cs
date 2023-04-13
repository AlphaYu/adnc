namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    public record UserTokenInfoDto : IDto
    {
        private UserTokenInfoDto()
        {
        }

        public UserTokenInfoDto(string token, DateTime exprie, string refreshToken, DateTime refreshExprie)
        {
            Token = token;
            Expire = exprie;
            RefreshToken = refreshToken;
            RefreshExpire = refreshExprie;
        }

        /// <summary>
        /// accesstoken
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// accesstoken exprie time
        /// </summary>
        public DateTime Expire { get; set; }

        /// <summary>
        /// refresh token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// refreshtoken exprie time
        /// </summary>
        public DateTime RefreshExpire { get; set; }
    }
}