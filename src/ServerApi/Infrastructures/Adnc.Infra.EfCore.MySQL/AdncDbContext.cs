using Adnc.Infra.Core.Guard;

namespace Adnc.Infra.EfCore.MySQL;

/// <summary>
/// AdncDbContext
/// </summary>
public class AdncDbContext : DbContext
{
    private readonly Operater _operater;
    private readonly IEntityInfo _entityInfo;
    private readonly UnitOfWorkStatus _unitOfWorkStatus;

    public AdncDbContext(DbContextOptions options, Operater operater, IEntityInfo entityInfo, UnitOfWorkStatus unitOfWorkStatus)
        : base(options)
    {
        _operater = operater;
        _entityInfo = entityInfo;
        _unitOfWorkStatus = unitOfWorkStatus;

        //关闭DbContext默认事务
        Database.AutoTransactionsEnabled = false;
        //关闭查询跟踪
        //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

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

    private int SetAuditFields()
    {
        var allBasicAuditEntities = ChangeTracker.Entries<IBasicAuditInfo>().Where(x => x.State == EntityState.Added).ToList();
        allBasicAuditEntities.ForEach(entry =>
        {
            entry.Entity.CreateBy = _operater.Id;
            entry.Entity.CreateTime = DateTime.Now;
        });

        var auditFullEntities = ChangeTracker.Entries<IFullAuditInfo>().Where(x => x.State == EntityState.Modified).ToList();
        auditFullEntities.ForEach(entry =>
        {
            entry.Entity.ModifyBy = _operater.Id;
            entry.Entity.ModifyTime = DateTime.Now;
        });

        return ChangeTracker.Entries<Entity>().Count();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //System.Diagnostics.Debugger.Launch();
        modelBuilder.HasCharSet("utf8mb4 ");

        var entityInfos = _entityInfo.GetEntitiesTypeInfo().ToList();
        Guard.Checker.NotNullOrAny(entityInfos, nameof(entityInfos));
        foreach (var info in entityInfos)
        {
            if (info.DataSeeding.IsNullOrEmpty())
                modelBuilder.Entity(info.Type);
            else
                modelBuilder.Entity(info.Type).HasData(info.DataSeeding);
        }

        var assembly = entityInfos.FirstOrDefault().Type.Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        var types = entityInfos.Select(x => x.Type);
        var entityTypes = modelBuilder.Model.GetEntityTypes().Where(x => types.Contains(x.ClrType)).ToList();
        entityTypes.ForEach(entityType =>
        {
            modelBuilder.Entity(entityType.Name, buider =>
            {
                var typeSummary = entityType.ClrType.GetSummary();
                buider.ToTable(entityType.ClrType.Name.ToLower()).HasComment(typeSummary);

                var properties = entityType.GetProperties().ToList();
                properties.ForEach(property =>
                {
                    var memberSummary = entityType.ClrType.GetMember(property.Name).FirstOrDefault().GetSummary();
                    buider.Property(property.Name)
                        .HasColumnName(property.Name.ToLower())
                        .HasComment(memberSummary);
                });
            });
        });
    }
}