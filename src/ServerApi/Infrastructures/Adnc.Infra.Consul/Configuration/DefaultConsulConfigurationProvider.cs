namespace Adnc.Infra.Consul.Configuration;

public sealed class DefaultConsulConfigurationProvider : ConfigurationProvider
{
    private readonly ConsulClient _consulClient;
    private readonly string _path;
    private readonly int _waitMillisecond;
    private readonly bool _reloadOnChange;
    private ulong _currentIndex;
    private Task? _pollTask;

    public DefaultConsulConfigurationProvider(ConsulClient consulClient, string consulKeyPath, bool reloadOnChanges)
    {
        _consulClient = consulClient;
        _path = consulKeyPath;
        _waitMillisecond = 1000*3;
        _reloadOnChange = reloadOnChanges;
    }

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
            || !queryResult.Response.Value.Any())
        {
            return;
        }
        Stream stream = new MemoryStream(queryResult.Response.Value);
        Data = JsonConfigurationFileParser.Parse(stream);
    }

    private async Task<QueryResult<KVPair>> GetData()
    {
        var res = await _consulClient.KV.Get(_path);
        if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.NotFound)
        {
            return res;
        }
        throw new Exception($"Error loading configuration from consul. Status code: {res.StatusCode}.");
    }

    private void PollReaload()
    {
        if (_reloadOnChange)
        {
            _pollTask = Task.Run(async () =>
            {
                while (true)
                {
                    QueryResult<KVPair> queryResult = await GetData();
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