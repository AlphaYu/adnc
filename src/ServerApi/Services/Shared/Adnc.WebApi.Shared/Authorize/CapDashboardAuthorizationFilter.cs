using DotNetCore.CAP.Dashboard;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    public class CapDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            //这里定义capdashboard外网验证，需要完善
            //默认内网可以访问
            if (context.Request.LocalIpAddress == "127.0.0.0.1")
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(true);
        }
    }
}