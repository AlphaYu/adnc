using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Services;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionHandlerLocal : PermissionHandler
    {
        private readonly IRoleAppService _roleAppService;


        public PermissionHandlerLocal(IRoleAppService roleAppService)
            : base()
        {
            _roleAppService = roleAppService;
        }

        protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes)
        {
            return await _roleAppService.ExistPermissionsAsync(userId, codes);
        }
    }
}
