namespace Adnc.Infra.Repository.EfCore;

public abstract class AdncDbContext : DbContext
{
    private readonly Operater _operater;
    private readonly IEntityInfo _entityInfo;

    protected AdncDbContext(DbContextOptions options, Operater operater, IEntityInfo entityInfo)
        : base(options)
    {
        _operater = operater;
        _entityInfo = entityInfo;
        Database.AutoTransactionsEnabled = false;
        //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changedEntities = SetAuditFields();

        //没有自动开启事务的情况下,保证主从表插入，主从表更新开启事务。
        var isManualTransaction = false;
        if (!Database.AutoTransactionsEnabled && Database.CurrentTransaction is null && changedEntities > 1)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entityInfos = _entityInfo.GetEntitiesTypeInfo();
        entityInfos?.ForEach(info =>
        {
            if (info.DataSeeding is null)
                modelBuilder.Entity(info.Type);
            else
                modelBuilder.Entity(info.Type).HasData(info.DataSeeding);
        });

        var assemblys = _entityInfo.GetConfigAssemblys();
        assemblys?.ForEach(assembly => modelBuilder.ApplyConfigurationsFromAssembly(assembly));

        SetComment(modelBuilder, entityInfos);
    }

    protected virtual int SetAuditFields()
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

    protected virtual void SetComment(ModelBuilder modelBuilder, IEnumerable<EntityTypeInfo>? entityInfos)
    {
        if (entityInfos is null)
            return;
        var types = entityInfos.Select(x => x.Type);
        var entityTypes = modelBuilder.Model.GetEntityTypes().Where(x => types.Contains(x.ClrType));
        entityTypes.ForEach(entityType =>
        {
            modelBuilder.Entity(entityType.Name, buider =>
            {
                var typeSummary = entityType.ClrType.GetSummary();
                buider.HasComment(typeSummary);

                entityType.GetProperties().ForEach(property =>
                {
                    string propertyName = property.Name;
                    var memberSummary = entityType.ClrType?.GetMember(propertyName)?.FirstOrDefault()?.GetSummary();
                    buider.Property(propertyName).HasComment(memberSummary);
                });
            });
        });
    }
}