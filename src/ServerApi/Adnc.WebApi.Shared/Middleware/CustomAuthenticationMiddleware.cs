using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Adnc.Infr.Common.Extensions;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Adnc.Infr.Common;
using EasyCaching.Core;
using Adnc.Application.Shared;

namespace Adnc.WebApi.Shared.Middleware
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserContext _currentUser;
        //private readonly IDistributedCache _cache;
        private readonly JWTConfig _jwtConfig;
        private readonly string tokenPrefx = "accesstoken:";
        private readonly string refreshTokenPrefx = "refreshtoken";
        private readonly IHybridCachingProvider _cache;

        public CustomAuthenticationMiddleware(RequestDelegate next
            , UserContext userContext
            //, IDistributedCache cache
            , IOptions<JWTConfig> jwtConfig
            , IHybridProviderFactory hybridProviderFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _currentUser = userContext ?? throw new ArgumentNullException(nameof(userContext));
            //_cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cache = hybridProviderFactory.GetHybridCachingProvider(BaseEasyCachingConsts.HybridCaching) ?? throw new ArgumentNullException(nameof(_cache));
            _jwtConfig = jwtConfig.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var requiredValues = ((RouteEndpoint)endpoint).RoutePattern.RequiredValues;
            if(requiredValues.Count()==0)
            {
                await _next(context);
                return;
            }

            var controller = requiredValues["controller"]?.ToString();
            var action = requiredValues["action"]?.ToString();
            if (string.IsNullOrEmpty(controller))
            {
                await _next(context);
                return;
            }

            string tokenTxt = string.Empty;
            string refreshTokenTxt = string.Empty;

            //读取Request
            //context.Request.EnableBuffering();
            //var requestReader = new StreamReader(context.Request.Body);
            //var requestContent = requestReader.ReadToEnd();
            //context.Request.Body.Position = 0;

            // Allow Anonymous skips all authorization
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {

                //如果是调用登陆API并且调用成功，需要保存accesstoken道cache
                if (string.Equals(controller, "account", StringComparison.InvariantCultureIgnoreCase)
                    && (string.Equals(action, "login", StringComparison.InvariantCultureIgnoreCase)
                    ||string.Equals(action, "refreshaccesstoken", StringComparison.InvariantCultureIgnoreCase))
                    )
                {
                    string responseContent;

                    var originalBodyStream = context.Response.Body;

                    using (var fakeResponseBody = new MemoryStream())
                    {
                        context.Response.Body = fakeResponseBody;

                        await _next(context);

                        fakeResponseBody.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(fakeResponseBody))
                        {
                            responseContent = await reader.ReadToEndAsync();
                            fakeResponseBody.Seek(0, SeekOrigin.Begin);

                            await fakeResponseBody.CopyToAsync(originalBodyStream);
                        }
                    }

                    if(context.Response.StatusCode == 200)
                    {
                        tokenTxt = JObject.Parse(responseContent).GetValue("token").ToString();
                        refreshTokenTxt = JObject.Parse(responseContent).GetValue("refreshToken").ToString();

                        var claimsInfo = GetClaimsInfo(tokenTxt);
                        if (!string.IsNullOrEmpty(claimsInfo.Account))
                        {
                            var key = $"{tokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                            //_cache.SetString(key, claimsInfo.Token, new DistributedCacheEntryOptions { AbsoluteExpiration = claimsInfo.Expire });
                            _cache.Set(key, claimsInfo.Token,  claimsInfo.Expire-DateTime.Now);
                            var refreshTokenKey = $"{refreshTokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                            //_cache.SetString(refreshTokenKey, refreshTokenTxt, new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddMinutes(_jwtConfig.RefreshTokenExpire) });
                            _cache.Set(refreshTokenKey, refreshTokenTxt, TimeSpan.FromSeconds(_jwtConfig.RefreshTokenExpire));
                        }
                    }
                }

                return;
            }


            tokenTxt = await context.GetTokenAsync("access_token");

            //如果是调用注销API并且调用成功，需要从cahce移除accesstoken
            if (string.Equals(controller, "account", StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(action, "logout", StringComparison.InvariantCultureIgnoreCase))
            {

                await _next(context);

                if (context.Response.StatusCode == 200)
                {
                    var claimsInfo = GetClaimsInfo(tokenTxt);
                    if (!string.IsNullOrEmpty(claimsInfo.Account))
                    {
                        var key = $"{tokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                        var refreshTokenKey = $"{refreshTokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";

                        //可以考虑事务操作，以后再优化
                        _cache.Remove(key);
                        _cache.Remove(refreshTokenKey);
                    }
                }

                return;
            }

            //如果是其他需要授权的API并且调用成功，需要从检查accesstoken是否在缓存中。
            if (context.Response.StatusCode == 200)
            {
                var claimsInfo = GetClaimsInfo(tokenTxt);
                if (!string.IsNullOrEmpty(claimsInfo.Account))
                {
                    var key = $"{tokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                    //var cahceToken = _cache.GetString(key);
                    var cahceToken = _cache.Get<string>(key).Value;
                    if (cahceToken != claimsInfo.Token)
                    {
                        await context.ForbidAsync();
                    }
                    else
                    {
                        await _next(context);
                    }
                }

                return;
            }
        }

        private (string Account,long Id,DateTime Expire,string Token) GetClaimsInfo(string tokenTxt)
        {
            if (!string.IsNullOrWhiteSpace(tokenTxt))
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenTxt);
                if (token != null)
                {
                    var expireTimestamp = token.Claims.Where(p => p.Type == "exp").FirstOrDefault().Value.ToLong();
                    var expireDt = expireTimestamp.Value.ToLocalTime().AddMinutes(_jwtConfig.ClockSkew);
                    var Id = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value.ToLong();
                    var account = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                    return (Account:account, Id:Id.Value, Expire: expireDt, Token:tokenTxt);
                }
            }

            return (null,0,DateTime.Now,null);
        }
    }
    //扩展方法
    public static class CustomAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthenticationMiddleware>();
        }
    }
}
