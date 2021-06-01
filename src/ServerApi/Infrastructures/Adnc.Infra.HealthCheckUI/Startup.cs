using Adnc.Infra.Consul.Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace Adnc.Maintaining
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var consul = new ConsulServiceProvider("http://193.112.75.77:8550");

            services.AddHealthChecksUI(setup =>
            {
                var services = consul.GetAllServicesAsync("adnc.usr.webapi").Result;
                if (services.Any())
                {
                    foreach (var item in services)
                    {
                        //setup.AddHealthCheckEndpoint(item.Service.ID, item.ServiceAddress);
                    }
                }
                setup.MaximumHistoryEntriesPerEndpoint(100);
            })
           .AddInMemoryStorage();
            //.AddSqliteStorage($"Data Source=sqlitehealthchecks.db");

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecksUI(setup => { setup.AddCustomStylesheet("dotnet.css"); });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}