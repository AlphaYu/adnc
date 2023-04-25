using System.Text;

namespace Adnc.Gateway.Ocelot
{
    public static class ApplicationBuilderExtension
    {
        public static string GetDefaultPageContent(this IApplicationBuilder app)
        {
            var configration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            var outputHtml = new StringBuilder($"Talk is cheap. Show me the code.", 500);
            outputHtml.AppendLine($"<br>ASPNETCORE_ENVIRONMENT={env.EnvironmentName}<br>");
            var serviceRoters = configration.GetSection("Routes").Get<List<ServiceRouter>>();
            if (serviceRoters is not null)
            {
                var routerDictionaryies = serviceRoters.Where(x => !x.Path.StartsWith("/auth")).GroupBy(x => x.Group);
                foreach (var group in routerDictionaryies)
                {
                    outputHtml.AppendLine($"<br>{group.Key}");
                    outputHtml.AppendLine($"<br>---------------------------------------------------------<br>");
                    foreach (var router in group)
                    {
                        outputHtml.AppendLine($"<a href='{router.Path}' target='_blank'>{router.ServiceName}</a>&nbsp;&nbsp;");
                    }
                }
            }
            return outputHtml.ToString();
        }
    }
}
