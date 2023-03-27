namespace Adnc.Shared.Rpc.Handlers;

public class CacheDelegatingHandler : DelegatingHandler
{
    private static readonly SemaphoreSlim _slimlock = new(1, 1);
    private readonly IMemoryCache _memoryCache;

    public CacheDelegatingHandler(IMemoryCache memoryCache) => _memoryCache = memoryCache;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken);
        //if (request.Method == HttpMethod.Get)
        //    return await HandleGetMethod(request, cancellationToken);
        //else
        //    return await base.SendAsync(request, cancellationToken);
    }

    /// <summary>
    /// cache handler 还需要测试
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    private async Task<HttpResponseMessage> HandleGetMethod(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var cacheHeader = request.Headers.FirstOrDefault(x => x.Key.EqualsIgnoreCase("cache"));
        if (cacheHeader.Key.IsNullOrWhiteSpace())
            return await base.SendAsync(request, cancellationToken);

        _ = int.TryParse(cacheHeader.Value.FirstOrDefault(), out int milliseconds);
        if (milliseconds <= 0)
            return await base.SendAsync(request, cancellationToken);

        if (request is null || request.RequestUri is null)
            throw new ArgumentNullException(nameof(request));

        var cacheKey = request.RequestUri.AbsoluteUri.GetHashCode();
        var existsCache = _memoryCache.TryGetValue(cacheKey, out string content);
        if (existsCache)
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content, Encoding.UTF8)
            };

        await _slimlock.WaitAsync(3000, cancellationToken);
        try
        {
            //SendAsync异常(请求、超时异常)，会throw；服务端异常，不会抛出
            var responseMessage = await base.SendAsync(request, cancellationToken);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var cacheValue = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                _memoryCache.Set(cacheKey, cacheValue, TimeSpan.FromMilliseconds(milliseconds));
            }
            return responseMessage;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            if (_slimlock.CurrentCount > 0)
                _slimlock.Release();
        }
    }
}