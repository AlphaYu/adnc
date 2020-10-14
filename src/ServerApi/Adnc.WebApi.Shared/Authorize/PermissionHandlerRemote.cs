using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Adnc.Core.Shared.RpcServices;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionHandlerRemote : PermissionHandler
    {
        private readonly IAuthRpcService _authRpcService;
        private readonly IHttpContextAccessor _contextAccessor;

        public PermissionHandlerRemote(IAuthRpcService authRpcService
            , IHttpContextAccessor contextAccessor) 
            : base()
        {
            _authRpcService = authRpcService;
            _contextAccessor = contextAccessor;
        }

        protected override async Task<bool> CheckUserPermissions(long userId, long[] roleIds, string[] codes)
        {
            var jwtToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            var permissions = await _authRpcService.GetCurrenUserPermissions($"Bearer {jwtToken}", userId, codes);
            bool result = permissions != null && permissions.Any() ? true : false;
            return result;
        }
    }
}
