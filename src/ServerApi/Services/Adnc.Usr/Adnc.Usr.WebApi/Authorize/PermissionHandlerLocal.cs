using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Services;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionHandlerLocal : PermissionHandler
    {
        private readonly IRoleAppService _roleAppService;

        public PermissionHandlerLocal(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes)
        {
            var permissions = await _roleAppService.GetPermissionsAsync(userId, codes);
            bool result = permissions != null && permissions.Any();
            return result;
        }
    }
}
