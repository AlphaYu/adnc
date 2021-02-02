using System;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Adnc.Usr.Application.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Helper
{
    //认证服务器安装：System.IdentityModel.Tokens.Jwt
    //资源服务器安装：Microsoft.AspNetCore.Authentication.JwtBearer
    public enum TokenType
    {
        AccessToken = 1,
        RefreshToken = 2
    }

    public class JwtTokenHelper
    {
        public static string CreateToken(JWTConfig jwtConfig, Claim[] claims, TokenType tokenType)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey));

            string issuer = jwtConfig.Issuer;
            string audience = tokenType.Equals(TokenType.AccessToken) ? jwtConfig.Audience : jwtConfig.RefreshTokenAudience;
            int expires = tokenType.Equals(TokenType.AccessToken) ? jwtConfig.Expire : jwtConfig.RefreshTokenExpire;

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(expires),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var jwtAccessTokenToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtAccessTokenToken;
        }

        public static string CreateAccessToken(JWTConfig jwtConfig, UserValidateDto user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId??"0")
            };
            return CreateToken(jwtConfig, claims, TokenType.AccessToken);
        }

        public static string CreateRefreshToken(JWTConfig jwtConfig, UserValidateDto user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Account),
            };
            return CreateToken(jwtConfig, claims, TokenType.RefreshToken);
        }

        public static string CreateAccessToken(JWTConfig jwtConfig, UserValidateDto user, string refreshTokenTxt)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenTxt);
            if (token != null)
            {
                var claimAccount = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                if (user != null && user.Account == claimAccount)
                {
                    return CreateAccessToken(jwtConfig, user);
                }

            }
            return string.Empty;
        }
    }
}
