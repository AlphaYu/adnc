using System.Text;

namespace Adnc.Gateway.Ocelot
{
    public static class ApplicationBuilderExtension
    {
        public static string GetDefaultPageContent(this IApplicationBuilder app)
        {
            var configration = app.ApplicationServices.GetRequiredService<IConfiguration>();
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            var outputHtml = new StringBuilder(500);
            outputHtml.Append("<div align='center'>");
            outputHtml.Append(@"<a href='https://github.com/alphayu/adnc' target='_blank'><img src='adnc-topics.png'/></a>");
            outputHtml.Append($"<br><br>ASPNETCORE_ENVIRONMENT={env.EnvironmentName}<br>");
            var serviceRoters = configration.GetSection("Routes").Get<List<ServiceRouter>>();
            if (serviceRoters is not null)
            {
                var routerDictionaryies = serviceRoters.Where(x => !x.Path.StartsWith("/auth")).GroupBy(x => x.Group);
                foreach (var group in routerDictionaryies)
                {
                    outputHtml.Append($"<br><b>{group.Key}</b>");
                    outputHtml.Append($"<br>--------------------------------------------------------------------------------------------<br>");
                    foreach (var router in group)
                    {
                        outputHtml.Append($"<a href='{router.Path}' target='_blank'>{router.ServiceName}</a>&nbsp;&nbsp;");
                    }
                }
            }
            outputHtml.Append($"<br><br>{DateTime.Now}");
            outputHtml.Append("</div>");
            return outputHtml.ToString();
        }
    }
}
