namespace Adnc.Infra.Entities
{
    public interface IEntity<TKey> : IEntityNotKey
    {
        public TKey Id { get; set; }
    }

    /// <summary>
    /// 无主键的实体接口
    /// </summary>
    public interface IEntityNotKey
    {

    }
}