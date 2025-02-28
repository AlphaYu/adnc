namespace Adnc.Demo.Usr.Application.Contracts.Dtos
{
    public record UserTokenInfoDto : IDto
    {
        public UserTokenInfoDto(string name, string token, DateTime exprie, string refreshToken, DateTime refreshExprie)
        {
            Name = name;
            Token = token;
            Expire = exprie;
            RefreshToken = refreshToken;
            RefreshExpire = refreshExprie;
        }

        /// <summary>
        /// username
        /// </summary>
        public string Name { get; set; }

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