using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        //Add any custom requirement properties if you have them
        public PermissionRequirement()
        {

        }
    }

}
