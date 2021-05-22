using Adnc.Application.Shared.RpcServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionHandlerRemote : PermissionHandler
    {
        private readonly IAuthRpcService _authRpcService;
        //private readonly IHttpContextAccessor _contextAccessor;

        public PermissionHandlerRemote(IAuthRpcService authRpcService)
        {
            _authRpcService = authRpcService;
            //_contextAccessor = contextAccessor;
        }

        protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes)
        {
            //var jwtToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            //var refitResult = await _authRpcService.GetCurrenUserPermissions($"Bearer {jwtToken}", userId, codes);
            var refitResult = await _authRpcService.GetCurrenUserPermissionsAsync(userId, codes);
            if (!refitResult.IsSuccessStatusCode)
                return false;

            var permissions = refitResult.Content;
            bool result = permissions != null && permissions.Any();
            return result;
        }
    }
}