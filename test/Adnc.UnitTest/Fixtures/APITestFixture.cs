namespace Adnc.UnitTest.Fixtures;

/// <summary>
/// Shared Context https://xunit.github.io/docs/shared-context.html
/// </summary>
public class APITestFixture : IDisposable
{
    //private readonly IWebHost _server;
    //public IServiceProvider Services { get; }
    //public HttpClient Client { get; }
    //public APITestFixture()
    //{
    //    var baseUrl = $"http://localhost:{GetRandomPort()}";
    //    _server = WebHost.CreateDefaultBuilder()
    //        .UseUrls(baseUrl)
    //        .UseStartup<TestStartup>()
    //        .Build();
    //    _server.Start();
    //    Services = _server.Services;
    //    Client = new HttpClient(new WeihanLi.Common.Http.NoProxyHttpClientHandler())
    //    {
    //        BaseAddress = new Uri($"{baseUrl}")
    //    };
    //    // Add Api-Version Header
    //    // Client.DefaultRequestHeaders.TryAddWithoutValidation("Api-Version", "1.2");
    //    Initialize();
    //    Console.WriteLine("test begin");
    //}
    ///// <summary>
    ///// TestDataInitialize
    ///// </summary>
    //private void Initialize()
    //{
    //}
    //public void Dispose()
    //{
    //    using (var dbContext = Services.GetRequiredService<ReservationDbContext>())
    //    {
    //        if (dbContext.Database.IsInMemory())
    //        {
    //            dbContext.Database.EnsureDeleted();
    //        }
    //    }
    //    Client.Dispose();
    //    _server.Dispose();
    //    Console.WriteLine("test end");
    //}
    //private static int GetRandomPort()
    //{
    //    var random = new Random();
    //    var randomPort = random.Next(10000, 65535);
    //    while (IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(p => p.Port == randomPort))
    //    {
    //        randomPort = random.Next(10000, 65535);
    //    }
    //    return randomPort;
    //}
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
