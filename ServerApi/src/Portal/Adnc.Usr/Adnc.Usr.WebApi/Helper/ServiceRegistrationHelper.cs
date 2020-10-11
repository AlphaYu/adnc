using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Adnc.Infr.EfCore.Interceptors;
using Adnc.Infr.Mongo;
using Adnc.Infr.Mongo.Extensions;
using Adnc.Infr.Mongo.Configuration;
using Adnc.Infr.EfCore;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Helper
{
    public sealed class ServiceRegistrationHelper : SharedServicesRegistration
    {
        public ServiceRegistrationHelper(IConfiguration configuration, IServiceCollection services)
          : base(configuration, services)
        {
        }

        public override void AddControllers()
        {
            _services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                        //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
        }

        public override void AddEfCoreContext()
        {
            _services.AddDbContext<AdncDbContext>(options =>
            {
                options.UseMySql(_mysqlConfig.WriteDbConnectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb));
                    mySqlOptions.MinBatchSize(2);
                });
                options.AddInterceptors(new CustomCommandInterceptor());
            });
        }

        public override void AddMongoContext()
        {
            _services.AddMongo<MongoContext>(options =>
            {
                options.ConnectionString = _mongoConfig.ConnectionStrings;
                options.PluralizeCollectionNames = _mongoConfig.PluralizeCollectionNames;
                options.CollectionNamingConvention = (NamingConvention)_mongoConfig.CollectionNamingConvention;
            });
        }

        public override void AddAuthorization()
        {
            _services.AddAuthorization(options =>
            {
                options.AddPolicy(Permission.Policy, policy =>
                    policy.Requirements.Add(new PermissionRequirement()));
            });
            
            _services.AddScoped<IAuthorizationHandler, PermissionHandlerLocal>();
        }
    }
}
