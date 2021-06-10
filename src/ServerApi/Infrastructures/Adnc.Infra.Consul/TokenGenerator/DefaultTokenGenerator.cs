using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Adnc.Infra.Consul.TokenGenerator
{
    public class DefaultTokenGenerator : ITokenGenerator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultTokenGenerator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Create()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var tokenTxt = token?.Remove(0, 7);
            return tokenTxt;
        }
    }
}