using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Services;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionHandlerLocal : PermissionHandler
    {
        private readonly IUserAppService _userAppService;

        public PermissionHandlerLocal(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes)
        {
            var permissions = await _userAppService.GetPermissionsAsync(userId, codes);
            bool result = permissions != null && permissions.Any();
            return result;
        }
    }
}
