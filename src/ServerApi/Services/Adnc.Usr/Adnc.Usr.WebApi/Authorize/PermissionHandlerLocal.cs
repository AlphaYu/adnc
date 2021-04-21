using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Adnc.Usr.Application.Contracts.Dtos;
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

        protected override async Task<bool> CheckUserPermissions(long userId, long[] roleIds, string[] codes)
        {
            var result = await _roleAppService.ExistPermissionsAsync(new RolePermissionsCheckerDto() { RoleIds = roleIds, Permissions = codes });
            if (result.IsSuccess)
                return result.Content;

            return false;
        }
    }
}
