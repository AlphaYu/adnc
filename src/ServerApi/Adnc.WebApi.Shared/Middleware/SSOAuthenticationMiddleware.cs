﻿using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using EasyCaching.Core;
using Newtonsoft.Json.Linq;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared;
using Adnc.Infr.Common.Helper;

namespace Adnc.WebApi.Shared.Middleware
{
    public class SSOAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTConfig _jwtConfig;
        private readonly string tokenPrefx = "accesstoken";
        //private readonly string refreshTokenPrefx = "refreshtoken";
        private readonly IHybridCachingProvider _cache;

        public SSOAuthenticationMiddleware(RequestDelegate next
            , IOptions<JWTConfig> jwtConfig
            , IHybridProviderFactory hybridProviderFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _cache = hybridProviderFactory.GetHybridCachingProvider(BaseEasyCachingConsts.HybridCaching) ?? throw new ArgumentNullException(nameof(_cache));
            _jwtConfig = jwtConfig.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var requiredValues = ((RouteEndpoint)endpoint).RoutePattern.RequiredValues;
            if (requiredValues.Count() == 0)
            {
                await _next(context);
                return;
            }

            var controller = requiredValues["controller"]?.ToString().ToLower();
            var action = requiredValues["action"]?.ToString().ToLower();
            if (controller.IsNullOrWhiteSpace())
            {
                await _next(context);
                return;
            }

            //判断Api是否需要认证
            bool isNeedAuthentication = endpoint.Metadata.GetMetadata<IAllowAnonymous>() == null ? true : false;

            //Api不需要认证
            if (isNeedAuthentication == false)
            {
                //如果是调用登录API、刷新token的Api,并且调用成功，需要保存accesstoken到cache
                if (controller == "account" && (action == "login" || action == "refreshaccesstoken"))
                {
                    await SaveToken(context, _next);
                    return;
                }

                //其他Api
                await _next(context);
                return;
            }

            //API需要认证
            if (isNeedAuthentication == true)
            {
                //是修改密码,需要从cahce移除Token
                if (controller == "account" && action == "password")
                {
                    await _next(context);
                    if (StatusCodeChecker.Is2xx(context.Response.StatusCode))
                        await RemoveToken(context);
                    return;
                }
                
                //是注销，需要判断是否主动注销
                if (controller == "account" && action == "logout")
                {
                    await _next(context);
                    if (StatusCodeChecker.Is2xx(context.Response.StatusCode))
                    {
                        //主动注销，从cahce移除token
                        if (await CheckToken(context) == true)
                            await RemoveToken(context);
                        return;
                    }
                }
            }

            //API需要认证，并且验证成功，需要检查accesstoken是否在缓存中。
            if (StatusCodeChecker.Is2xx(context.Response.StatusCode))
            {
                //需要先检查token是否是最新的，再走其它中间件
                var result = await CheckToken(context);
                if (result)
                {
                    try
                    {
                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                    return;
                }
                else
                {
                    var status = (int)HttpStatusCode.Unauthorized;
                    var hostAndPort = context.Request.Host.HasValue ? context.Request.Host.Value : string.Empty;
                    var requestUrl = string.Concat(hostAndPort, context.Request.Path);
                    var type = string.Concat("https://httpstatuses.com/", status);
                    var title = "Token已经过期";
                    var detial = "Token已经过期,请重新登录";
                    var problemDetails = new ProblemDetails
                    {
                        Title = title
                        ,
                        Detail = detial
                        ,
                        Type = type
                        ,
                        Status = status
                        ,
                        Instance = requestUrl
                    };
                    context.Response.StatusCode = status;
                    context.Response.ContentType = "application/problem+json";
                    var stream = context.Response.Body;
                    await JsonSerializer.SerializeAsync(stream, problemDetails);
                    return;
                }
            }

            await _next(context);
            return;
        }

        /// <summary>
        /// 保存token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private async Task SaveToken(HttpContext context, RequestDelegate next)
        {
            string responseContent;

            var originalBodyStream = context.Response.Body;

            using (var fakeResponseBody = new MemoryStream())
            {
                context.Response.Body = fakeResponseBody;

                await next(context);

                fakeResponseBody.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(fakeResponseBody))
                {
                    responseContent = await reader.ReadToEndAsync();
                    fakeResponseBody.Seek(0, SeekOrigin.Begin);

                    await fakeResponseBody.CopyToAsync(originalBodyStream);
                }
            }

            if (StatusCodeChecker.Is2xx(context.Response.StatusCode))
            {
                var tokenTxt = JObject.Parse(responseContent).GetValue("token")?.ToString();
                if (tokenTxt.IsNullOrWhiteSpace())
                    return;
                //refreshTokenTxt = JObject.Parse(responseContent).GetValue("refreshToken").ToString();

                var claimsInfo = GetClaimsInfo(tokenTxt);
                if (claimsInfo.Account.IsNotNullOrWhiteSpace())
                {
                    var tokenKey = $"{tokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                    _cache.Set(tokenKey, claimsInfo.Token, claimsInfo.Expire - DateTime.Now);
                    //var refreshTokenKey = $"{refreshTokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                    //_cache.Set(refreshTokenKey, refreshTokenTxt, TimeSpan.FromSeconds(_jwtConfig.RefreshTokenExpire));
                }
            }
        }

        /// <summary>
        /// 移除token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task RemoveToken(HttpContext context)
        {
            var tokenTxt = await context.GetTokenAsync("access_token");
            var claimsInfo = GetClaimsInfo(tokenTxt);
            if (claimsInfo.Account.IsNotNullOrWhiteSpace())
            {
                var tokenKey = $"{tokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                //var refreshTokenKey = $"{refreshTokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                //await _cache.RemoveAllAsync(new List<string>() { key, refreshTokenKey });
                await _cache.RemoveAsync(tokenKey);
            }
        }

        /// <summary>
        /// 检查token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<bool> CheckToken(HttpContext context)
        {
            var tokenTxt = await context.GetTokenAsync("access_token");
            var claimsInfo = GetClaimsInfo(tokenTxt);
            if (claimsInfo.Account.IsNotNullOrEmpty())
            {
                var tokenKey = $"{tokenPrefx}:{claimsInfo.Account}:{claimsInfo.Id}";
                var cahceToken = _cache.Get<string>(tokenKey).Value;
                return (cahceToken == claimsInfo.Token);
            }
            return false;
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="tokenTxt"></param>
        /// <returns></returns>
        private (string Account, long Id, DateTime Expire, string Token) GetClaimsInfo(string tokenTxt)
        {
            if (tokenTxt.IsNotNullOrWhiteSpace())
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenTxt);
                if (token != null)
                {
                    var expireTimestamp = token.Claims.Where(p => p.Type == "exp").FirstOrDefault().Value.ToLong();
                    var expireDt = expireTimestamp.Value.ToLocalTime().AddMinutes(_jwtConfig.ClockSkew);
                    var Id = token.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value.ToLong();
                    var account = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                    return (Account: account, Id: Id.Value, Expire: expireDt, Token: tokenTxt);
                }
            }

            return (null, 0, DateTime.Now, null);
        }
    }

    /// <summary>
    /// 注册单点登录中间件
    /// </summary>
    public static class SSOAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseSSOAuthentication(this IApplicationBuilder builder, bool isOpenSSOAuthentication = true)
        {
            if(isOpenSSOAuthentication)
                return builder.UseMiddleware<SSOAuthenticationMiddleware>();

            return builder;
        }
    }
}
