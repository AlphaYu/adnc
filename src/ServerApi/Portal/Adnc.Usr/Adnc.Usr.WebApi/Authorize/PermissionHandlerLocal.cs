using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Application.Services;

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

        protected override async Task<bool> CheckUserPermissions(long userId, long[] roleIds, string[] codes)
        {
            var result = await _roleAppService.ExistPermissions(new RolePermissionsCheckInputDto() { RoleIds = roleIds, Permissions = codes });
            if (result.IsSuccess)
                return result.Content;

            return false;
        }
    }
}
