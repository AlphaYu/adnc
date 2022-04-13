using Adnc.Infra.Core.Interfaces;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace Hangfire
{
    public static class HangfireApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder builder, IServiceInfo serviceInfo)
        {
            var options = new DashboardOptions
            {
                AppPath = $"/{serviceInfo.ShortName}/hangfire",
                DashboardTitle = $"{serviceInfo.Description} Jobs Dashboard",
                DisplayStorageConnectionString = false
            };

            builder.UseHangfireDashboard($"/{serviceInfo.ShortName}/hangfire", options);

            return builder;
        }

        public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder builder, IServiceInfo serviceInfo, params Authorize[] authorize)
        {
            var options = new DashboardOptions
            {
                AppPath = $"/{serviceInfo.ShortName}/hangfire",
                DashboardTitle = $"{serviceInfo.Description} Jobs Dashboard",
                DisplayStorageConnectionString = false
            };
            if (authorize.Any())
            {
                options.Authorization = authorize.Select(s =>
                                        new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                                        {
                                            Users = new[] { new BasicAuthAuthorizationUser { Login = s.Login, PasswordClear = s.Password } }
                                        })).ToArray();
            }
            builder.UseHangfireDashboard($"/{serviceInfo.ShortName}/hangfire", options);

            return builder;
        }
    }
}