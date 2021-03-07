using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;
using Adnc.Core.Shared.Entities;
using Adnc.Infr.Common;

namespace Adnc.Infr.EfCore
{
    public class AdncDbContext : DbContext
    {
        private readonly UserContext _userContext;
        private readonly IEntityInfo _entityInfo;
        private readonly UnitOfWorkStatus _unitOfWorkStatus;

        public AdncDbContext([NotNull] DbContextOptions options, UserContext userContext, [NotNull] IEntityInfo entityInfo, UnitOfWorkStatus unitOfWorkStatus)
            : base(options)
        {
            _userContext = userContext;
            _entityInfo = entityInfo;
            _unitOfWorkStatus = unitOfWorkStatus;

            //关闭DbContext默认事务
            Database.AutoTransactionsEnabled = false;
            //关闭查询跟踪
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            //生成数据库表有下面的三种方式：
            //1、代码生生成数据库
            //Database.EnsureCreated();
            //2、程序包管理器控制台迁移
            //(1)nuge安装Microsoft.EntityFrameworkCore.Tools包
            //(2)迁移命令(efcore工程) Add-Migration Init_First 生成sql
            //(3)更新数据库  Update-Database
            //3命令行迁移
            //(1)、进入EFCore目录
            //(2)、dotnet ef migrations add Initial
            //(3)、dotnet ef database update
        }

        //public override int SaveChanges()
        //{
        //    this.SetAuditFields();
        //    return base.SaveChanges();
        //}

        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{
        //    this.SetAuditFields();
        //    return base.SaveChanges(acceptAllChangesOnSuccess);
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changedEntities = this.SetAuditFields();

            //没有自动开启事务的情况下,保证主从表插入，主从表更新开启事务。
            var isManualTransaction = false;
            if (!Database.AutoTransactionsEnabled && !_unitOfWorkStatus.IsStartingUow && changedEntities > 1)
            {
                isManualTransaction = true;
                Database.AutoTransactionsEnabled = true;
            }

            var result = base.SaveChangesAsync(cancellationToken);

            //如果手工开启了自动事务，用完后关闭。
            if (isManualTransaction)
                Database.AutoTransactionsEnabled = false;

            return result;
        }

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    this.SetAuditFields();
        //    return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

        private int SetAuditFields()
        {
            var allEntities = ChangeTracker.Entries<Entity>();

            var allBasicAuditEntities = ChangeTracker.Entries<IBasicAuditInfo>().Where(x => x.State == EntityState.Added);
            foreach (var entry in allBasicAuditEntities)
            {
                var entity = entry.Entity;
                {
                    entity.CreateBy = _userContext.Id;
                    entity.CreateTime = DateTime.Now;
                }
            }

            var auditFullEntities = ChangeTracker.Entries<IFullAuditInfo>().Where(x => x.State == EntityState.Modified);
            foreach (var entry in auditFullEntities)
            {
                var entity = entry.Entity;
                {
                    entity.ModifyBy = _userContext.Id;
                    entity.ModifyTime = DateTime.Now;
                }
            }

            return allEntities.Count();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entitiesTypes = _entityInfo.GetEntities();

            foreach (var entityType in entitiesTypes)
            {
                modelBuilder.Entity(entityType);
            }

            //种子数据
            //modelBuilder.Entity<SysLoginLog>().HasData(new SysLoginLog{});
            //种子数据初始化方法分为如下三种
            //1、modelBuilder.Entity<>().HasData()方法，context.Database.EnsureCreated()只会执行一次
            //新增新的种子后，需要调用context.Database.Migrate方法调用生成的迁移类才能对数据的更改有效
            //2、控制台命令

            //生成迁移sql
            //Script-Migration -From migrationName1 -To migrationName2  -Context ContextName
            //如：Script-Migration -From 0

            modelBuilder.ApplyConfigurationsFromAssembly(_entityInfo.GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //用于设置是否启用缓存，暂时解决了可能出现的内存溢出的问题
            optionsBuilder.EnableServiceProviderCaching(false);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
