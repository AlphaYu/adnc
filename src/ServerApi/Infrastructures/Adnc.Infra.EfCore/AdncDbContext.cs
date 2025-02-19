namespace Adnc.Infra.Repository.EfCore;

public abstract class AdncDbContext : DbContext
{
    private readonly IEntityInfo _entityInfo;
    private readonly Operater _operater;

    protected AdncDbContext(DbContextOptions options, IEntityInfo entityInfo, Operater operater)
        : base(options)
    {
        _entityInfo = entityInfo;
        _operater = operater;
        Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
        //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //efcore7 support this feature
        //Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
        var changedEntities = SetAuditFields();
        var result = base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => _entityInfo.OnModelCreating(modelBuilder);

    protected virtual int SetAuditFields()
    {
        var allBasicAuditEntities = ChangeTracker.Entries<IBasicAuditInfo>().Where(x => x.State == EntityState.Added);
        allBasicAuditEntities.ForEach(entry =>
        {
            entry.Entity.CreateBy = _operater.Id;
            entry.Entity.CreateTime = DateTime.Now;
        });

        var auditFullEntities = ChangeTracker.Entries<IFullAuditInfo>().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added);
        auditFullEntities.ForEach(entry =>
        {
            entry.Entity.ModifyBy = _operater.Id;
            entry.Entity.ModifyTime = DateTime.Now;
        });

        return ChangeTracker.Entries<Entity>().Count();
    }
}