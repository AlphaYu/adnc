namespace Adnc.Infra.Entities;

public interface IEntityInfo
{
    void OnModelCreating(dynamic modelBuilder);
}