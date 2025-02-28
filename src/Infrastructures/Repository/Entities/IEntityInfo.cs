namespace Adnc.Infra.Repository;

public interface IEntityInfo
{
    void OnModelCreating(dynamic modelBuilder);
}