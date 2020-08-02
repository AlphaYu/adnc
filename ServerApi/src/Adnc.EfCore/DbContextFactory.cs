using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Concurrent;
using System.Threading;
using Adnc.Common.Models;

namespace  Adnc.Infr.EfCore
{
    public enum WirteOrRead
    {
        Write,
        Read
    }

    public interface IDbContextFactory
    {
        public DbContext CreateDbContext(WirteOrRead dbType);
    }

    public class DbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;
        private readonly UserContext _userContext;
        public DbContextFactory(IConfiguration configuration,UserContext userContext)
        {
            _configuration = configuration;
            _userContext = userContext;
        }

        public DbContext CreateDbContext(WirteOrRead dbType)
        {
            DbContext dbContext = null;
            if (dbType == WirteOrRead.Write)
            {
                dbContext = CallContext.GetData(WirteOrRead.Write);

                if (dbContext == null)
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AdncDbContext>();
                    optionsBuilder.UseMySql(_configuration.GetValue<string>("Mysql:WriteDb:ConnectionString")
                        , mySqlOptions => mySqlOptions.ServerVersion(new ServerVersion(new Version("5.5.56-MariaDB"), ServerType.MySql))
                    );
                    dbContext = new AdncDbContext(optionsBuilder.Options, _userContext);           
                    CallContext.SetData(WirteOrRead.Write, dbContext);
                }
            }
            return dbContext;
        }
    }
    
    public class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<DbContext>> state = new ConcurrentDictionary<string, AsyncLocal<DbContext>>();

        public static void SetData(WirteOrRead name, DbContext data) =>
            state.GetOrAdd(name.ToString(), _ => new AsyncLocal<DbContext>()).Value = data;

        public static DbContext GetData(WirteOrRead name) =>
            state.TryGetValue(name.ToString(), out AsyncLocal<DbContext> data) ? data.Value : null;
    }
}
