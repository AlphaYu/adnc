using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Configuration
{
    /// <summary>
    /// https://www.natmarchand.fr/consul-configuration-aspnet-core/
    /// </summary>
    public class ConsulConfigurationProvider : ConfigurationProvider
    {
        private const string ConsulIndexHeader = "X-Consul-Index";

        private readonly string _path;
        private readonly HttpClient _httpClient;
        private readonly IReadOnlyList<Uri> _consulUrls;
        private readonly Task _configurationListeningTask;
        private int _consulUrlIndex;
        private int _failureCount;
        private int _consulConfigurationIndex;

        public ConsulConfigurationProvider(IEnumerable<Uri> consulUrls, string path)
        {
            _path = path;
            _consulUrls = consulUrls.Select(u => new Uri(u, $"v1/kv/{path}")).ToList();

            if (_consulUrls.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(consulUrls));
            }

            _httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }, true);
            _configurationListeningTask = new Task(ListenToConfigurationChanges);
        }

        public override void Load() => LoadAsync().ConfigureAwait(false).GetAwaiter().GetResult();

        private async Task LoadAsync()
        {
            Data = await ExecuteQueryAsync();

            if (_configurationListeningTask.Status == TaskStatus.Created)
                _configurationListeningTask.Start();
        }

        private async void ListenToConfigurationChanges()
        {
            while (true)
            {
                try
                {
                    if (_failureCount > _consulUrls.Count)
                    {
                        _failureCount = 0;
                        await Task.Delay(TimeSpan.FromMinutes(1));
                    }

                    Data = await ExecuteQueryAsync(true);
                    OnReload();
                    _failureCount = 0;
                }
                catch (TaskCanceledException)
                {
                    _failureCount = 0;
                }
                catch
                {
                    _consulUrlIndex = (_consulUrlIndex + 1) % _consulUrls.Count;
                    _failureCount++;
                }
            }
        }

        private async Task<IDictionary<string, string>> ExecuteQueryAsync(bool isBlocking = false)
        {
            IDictionary<string, string> result;
            var relativeUri = isBlocking ? $"?recurse=true&index={_consulConfigurationIndex}" : "?recurse=true";
            var fullUri = string.Empty;
            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_consulUrls[_consulUrlIndex], relativeUri)))
            {
                fullUri = request.RequestUri.ToString();
                try
                {
                    using (var response = await _httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        if (response.Headers.Contains(ConsulIndexHeader))
                        {
                            var indexValue = response.Headers.GetValues(ConsulIndexHeader).FirstOrDefault();
                            int.TryParse(indexValue, out _consulConfigurationIndex);
                        }

                        var tokens = JToken.Parse(await response.Content.ReadAsStringAsync());
                        result = tokens
                            .Select(k => KeyValuePair.Create
                                (
                                    //k.Value<string>("Key").Substring(_path.Length + 1),
                                    k.Value<string>("Key"),
                                    k.Value<string>("Value") != null ? JToken.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(k.Value<string>("Value")))) : null
                                ))
                            .Where(v => !string.IsNullOrWhiteSpace(v.Key))
                            .SelectMany(Flatten)
                            .ToDictionary(v => ConfigurationPath.Combine(v.Key.Split('/')), v => v.Value, StringComparer.OrdinalIgnoreCase);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}({fullUri})", ex);
                }
            }

            return result;
        }

        private IEnumerable<KeyValuePair<string, string>> Flatten(KeyValuePair<string, JToken> tuple)
        {
            if (!(tuple.Value is JObject value))
                yield break;

            foreach (var property in value)
            {
                var propertyKey = (tuple.Key == _path) ? property.Key : $"{tuple.Key}/{property.Key}";
                //var propertyKey = $"{tuple.Key}/{property.Key}";
                switch (property.Value.Type)
                {
                    case JTokenType.Object:
                        foreach (var item in Flatten(KeyValuePair.Create(propertyKey, property.Value)))
                            yield return item;
                        break;
                    case JTokenType.Array:
                        {
                            int index = 0;
                            foreach (var item in property.Value)
                            {
                                propertyKey = $"{propertyKey}:{index}";
                                index += 1;
                                yield return KeyValuePair.Create(propertyKey, item.Value<string>()); ;
                            }
                            break;
                        }
                    default:
                        yield return KeyValuePair.Create(propertyKey, property.Value.Value<string>());
                        break;
                }
            }
        }
    }
}
