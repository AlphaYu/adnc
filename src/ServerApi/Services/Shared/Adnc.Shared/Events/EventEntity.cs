namespace Adnc.Shared.Events
{
    [Serializable]
    public class EventEntity<TData> where TData : class
    {
        public EventEntity()
        {
            this.Id = default;
            this.Data = default!;
            this.OccurredDate = DateTime.Now;
            this.EventSource = string.Empty;
            this.EventTarget = string.Empty;
        }

        public EventEntity(long id, TData data, string source)
        {
            this.Id = id;
            this.Data = data;
            this.OccurredDate = DateTime.Now;
            this.EventSource = source;
            this.EventTarget = string.Empty;
        }

        /// <summary>
        /// 事件Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public virtual DateTime OccurredDate { get; set; }

        /// <summary>
        /// 触发事件的方法
        /// </summary>
        public virtual string EventSource { get; set; }

        /// <summary>
        /// 处理事件的方法
        /// </summary>
        public virtual string EventTarget { get; set; }

        /// <summary>
        /// 事件数据
        /// </summary>
        public TData Data { get; set; }
    }
}