using Adnc.Infra.IRepositories;

namespace Adnc.Infra.Entities;

public interface IEntityInfo
{
    Operater GetOperater();

    void OnModelCreating(dynamic modelBuilder);
}