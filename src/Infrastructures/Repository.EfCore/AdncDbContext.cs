namespace Adnc.Infra.Repository.EfCore;

public abstract class AdncDbContext(DbContextOptions options, IEntityInfo entityInfo, Operater operater) : DbContext(options)
{
    private readonly IEntityInfo _entityInfo = entityInfo;
    private readonly Operater _operater = operater;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //efcore7 support this feature
        //Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
        // var changedEntities = SetAuditFields();
        var result = base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
        //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _entityInfo.OnModelCreating(modelBuilder);
    }

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