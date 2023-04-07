using DotNetCore.CAP;

namespace Adnc.Infra.EventBus.Cap
{
    /// <summary>
    /// 空的Cap发布者，给单元测试用
    /// </summary>
    public class NullCapPublisher : ICapPublisher
    {
        public IServiceProvider ServiceProvider { get; } = default!;

        public AsyncLocal<ICapTransaction> Transaction { get; } = default!;

        public void Publish<T>(string name, T? contentObj, string? callbackName = null)
        {
            // Method intentionally left empty.
        }

        public void Publish<T>(string name, T? contentObj, IDictionary<string, string?> headers)
        {
            // Method intentionally left empty.
        }

        public Task PublishAsync<T>(string name, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task PublishAsync<T>(string name, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}