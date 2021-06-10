namespace Adnc.Infra.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}