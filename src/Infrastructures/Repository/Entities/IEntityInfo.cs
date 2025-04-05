using Microsoft.EntityFrameworkCore;

namespace Adnc.Infra.Repository;

public interface IEntityInfo
{
    void OnModelCreating(ModelBuilder modelBuilder);
}
