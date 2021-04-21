using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Adnc.Infra.Consul
{
    public class DefaultTokenGenerator : ITokenGenerator
    {
        private IHttpContextAccessor _httpContextAccessor;
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
