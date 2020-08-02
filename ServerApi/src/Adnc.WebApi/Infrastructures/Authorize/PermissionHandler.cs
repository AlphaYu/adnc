using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;
using Adnc.Application.Services;
using Adnc.Application.Dtos;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {

        private readonly IRoleAppService _roleAppService;

        public PermissionHandler(IRoleAppService roleAppService)
            : base()
        {
            _roleAppService = roleAppService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if(!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var roles = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (string.IsNullOrWhiteSpace(roles))
            {
                context.Fail();
                return;
            }

            var roleIds = roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
            if (roleIds.Contains(1))
            {
                context.Succeed(requirement);
                return;
            }
            else
            {
                var attribute = (context.Resource as RouteEndpoint).Metadata.GetMetadata<PermissionAttribute>();

                var result = await _roleAppService.ExistPermissions(new RolePermissionsCheckInputDto() { RoleIds = roleIds.ToArray(), Permissions = attribute.Codes });

                if (result)
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            context.Fail();
            return;
        }
    }
}
