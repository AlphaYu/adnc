namespace Adnc.Infra.EventBus;

public interface IEventPublisher
{
    /// <summary>
    /// Asynchronously publishes an object message.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T contentObj, string? callbackName = null, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Asynchronously publishes an object message with custom headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Publishes an object message.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    void Publish<T>(T contentObj, string? callbackName = null) where T : class;

    /// <summary>
    /// Publishes an object message with custom headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    void Publish<T>(T contentObj, IDictionary<string, string?> headers) where T : class;

    /// <summary>
    /// Asynchronously schedules a message to be published at a future time with headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously schedules a message to be published at a future time.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules a message to be published at a future time with headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    void PublishDelay<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers);

    /// <summary>
    /// Schedules a message to be published at a future time.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    void PublishDelay<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null);

    /// <summary>
    /// Asynchronously publishes an object message.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(string name, T? contentObj, string? callbackName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously publishes an object message with custom headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(string name, T? contentObj, IDictionary<string, string?> headers,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an object message.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    void Publish<T>(string name, T? contentObj, string? callbackName = null);

    /// <summary>
    /// Publishes an object message with custom headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    void Publish<T>(string name, T? contentObj, IDictionary<string, string?> headers);

    /// <summary>
    /// Asynchronously schedules a message to be published at a future time with headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, string name, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously schedules a message to be published at a future time.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishDelayAsync<T>(TimeSpan delayTime, string name, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Schedules a message to be published at a future time with headers.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="headers">The message additional headers.</param>
    void PublishDelay<T>(TimeSpan delayTime, string name, T? contentObj, IDictionary<string, string?> headers);

    /// <summary>
    /// Schedules a message to be published at a future time.
    /// </summary>
    /// <typeparam name="T">The type of the message content object.</typeparam>
    /// <param name="delayTime">The delay for the message to be published.</param>
    /// <param name="name">The topic name or exchange router key.</param>
    /// <param name="contentObj">The message body content that will be serialized. (can be null)</param>
    /// <param name="callbackName">The callback subscriber name.</param>
    void PublishDelay<T>(TimeSpan delayTime, string name, T? contentObj, string? callbackName = null);
}
