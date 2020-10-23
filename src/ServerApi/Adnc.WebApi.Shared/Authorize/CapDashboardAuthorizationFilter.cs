using DotNetCore.CAP.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public class CapDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            //需要完善
            if (context.Request.LocalIpAddress == "127.0.0.0.1")
            {
                return await new ValueTask<bool>(true);
            }
            return await new ValueTask<bool>(true);
        }
    }
}
