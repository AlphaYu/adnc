namespace Adnc.Infra.Repository;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}
