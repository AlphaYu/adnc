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
    /// Event ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Time when the event occurred
    /// </summary>
    public DateTime OccurredDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Method that triggered the event
    /// </summary>
    public string EventSource { get; set; } = string.Empty;

    /// <summary>
    /// Method that handles the event
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
    /// Event payload
    /// </summary>
    public TData Data { get; set; } = default!;
}
