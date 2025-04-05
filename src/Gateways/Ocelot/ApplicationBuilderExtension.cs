namespace Adnc.Gateway.Ocelot;

public static class ApplicationBuilderExtension
{
    public static string GetDefaultPageContent(this IApplicationBuilder app)
    {
        var configration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        var outputHtml = new StringBuilder(500);
        outputHtml.Append("<div align='center'>");
        outputHtml.Append(@"<a href='https://github.com/alphayu/adnc' target='_blank'><img src='adnc-topics.png'/></a>");
        outputHtml.Append($"<br><br>ASPNETCORE_ENVIRONMENT={env.EnvironmentName}");
        var serviceRoters = configration.GetSection("Routes").Get<List<ServiceRouter>>();
        if (serviceRoters is not null)
        {
            outputHtml.Append($"<br>--------------------------------------------------------------------------------------------<br>");
            foreach (var router in serviceRoters)
            {
                if (router.Name.Contains("auth"))
                {
                    continue;
                }

                outputHtml.Append($"<a href='{router.Path}' target='_blank'>{router.Name}</a>&nbsp;&nbsp;<b>");
            }
        }
        outputHtml.Append($"<br><br>{DateTime.Now}");
        outputHtml.Append("</div>");
        return outputHtml.ToString();
    }
}
