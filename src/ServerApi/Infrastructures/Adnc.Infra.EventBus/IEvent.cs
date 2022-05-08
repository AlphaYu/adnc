namespace Adnc.Infra.EventBus
{
    public interface IEvent
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public DateTime OccurredDate { get; set; }

        /// <summary>
        /// 触发事件的对象
        /// </summary>
        public string EventSource { get; set; }
    }
}