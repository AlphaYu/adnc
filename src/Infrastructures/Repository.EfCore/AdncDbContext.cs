namespace Adnc.Infra.Repository.EfCore;

public abstract class AdncDbContext(DbContextOptions options, IEntityInfo entityInfo, Operater operater) : DbContext(options)
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //efcore7 support this feature , default is WhenNeeded
        //Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
        SetAuditFields();
        var result = base.SaveChangesAsync(cancellationToken);
        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        entityInfo.OnModelCreating(modelBuilder);
    }

    protected virtual void SetAuditFields()
    {
        var allBasicAuditEntities = ChangeTracker.Entries<IBasicAuditInfo>().Where(x => x.State == EntityState.Added);
        allBasicAuditEntities.ForEach(entry =>
        {
            entry.Entity.CreateBy = operater.Id;
            entry.Entity.CreateTime = DateTime.Now;
        });

        var auditFullEntities = ChangeTracker.Entries<IFullAuditInfo>().Where(x => x.State == EntityState.Modified || x.State == EntityState.Added);
        auditFullEntities.ForEach(entry =>
        {
            entry.Entity.ModifyBy = operater.Id;
            entry.Entity.ModifyTime = DateTime.Now;
        });
    }
}
