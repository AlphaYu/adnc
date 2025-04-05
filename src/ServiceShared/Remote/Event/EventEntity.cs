namespace Adnc.Shared.Remote.Event;

[Serializable]
public class EventEntity
{
    public EventEntity()
    {
    }

    public EventEntity(long id, string source)
    {
        Id = id;
        EventSource = source;
    }

    /// <summary>
    /// 事件Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 事件发生的时间
    /// </summary>
    public DateTime OccurredDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 触发事件的方法
    /// </summary>
    public string EventSource { get; set; } = string.Empty;

    /// <summary>
    /// 处理事件的方法
    /// </summary>
    public string EventTarget { get; set; } = string.Empty;
}

[Serializable]
public class EventEntity<TData> : EventEntity
    where TData : class
{
    public EventEntity()
        : base()
    {
    }

    public EventEntity(long id, TData data, string source)
        : base(id, source)
    {
        Data = data;
    }

    /// <summary>
    /// 事件数据
    /// </summary>
    public TData Data { get; set; } = default!;
}
