namespace Adnc.Infra.Consul.Configuration;

public sealed class DefaultConsulConfigurationProvider(ConsulClient consulClient, string consulKeyPath, bool reloadOnChanges) : ConfigurationProvider
{
    private readonly int _waitMillisecond = 1000 * 3;
    private ulong _currentIndex;
    private Task? _pollTask;

    public override void Load()
    {
        if (_pollTask != null)
        {
            return;
        }
        LoadData(GetData().GetAwaiter().GetResult());
        PollReaload();
    }

    private void LoadData(QueryResult<KVPair> queryResult)
    {
        _currentIndex = queryResult.LastIndex;
        if (queryResult.Response == null
            || queryResult.Response.Value == null
            || queryResult.Response.Value.Length == 0)
        {
            return;
        }
        Stream stream = new MemoryStream(queryResult.Response.Value);
        Data = JsonConfigurationFileParser.Parse(stream);
    }

    private async Task<QueryResult<KVPair>> GetData()
    {
        var res = await consulClient.KV.Get(consulKeyPath);
        if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.NotFound)
        {
            return res;
        }
        throw new InvalidOperationException($"Error loading configuration from consul. Status code: {res.StatusCode}.");
    }

    private void PollReaload()
    {
        if (reloadOnChanges)
        {
            _pollTask = Task.Run(async () =>
            {
                while (true)
                {
                    var queryResult = await GetData();
                    if (queryResult.LastIndex != _currentIndex)
                    {
                        LoadData(queryResult);
                        OnReload();
                    }
                    await Task.Delay(_waitMillisecond);
                }
            });
        }
    }
}
