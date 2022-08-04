namespace Adnc.Infra.Entities
{
    public class Entity : IEntity<long>
    {
        public long Id { get; set; }
    }
    /// <summary>
    /// 无键实体抽象类，视图、存储过程、函数依赖抽象类
    /// </summary>
    public class EntityNotKey : IEntityNotKey
    {

    }
}