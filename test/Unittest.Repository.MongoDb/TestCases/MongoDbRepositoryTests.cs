using Adnc.Infra.Unittest.Reposity.MongoDb.Fixtures;
using Adnc.Infra.Unittest.Reposity.MongoDb.Fixtures.Entities;
using MongoDB.Bson;

namespace Adnc.Infra.Unittest.Reposity.MongoDb.TestCases;

public class MongoDbRepositoryTests : IClassFixture<EfCoreMongoDbcontextFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IMongoDbRepository<LoggerLog> _logRsp;
    private readonly EfCoreMongoDbcontextFixture _fixture;

    public MongoDbRepositoryTests(EfCoreMongoDbcontextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _logRsp = _fixture.Container.GetRequiredService<IMongoDbRepository<LoggerLog>>();
    }

    [Fact]
    public async Task TestInsertsingleAndFetch()
    {
        var log = await GenerateLoggerLog();
        await _logRsp.InsertAsync(log);

        var result = await _logRsp.FetchAsync(x => x.Id == log.Id);

        Assert.NotNull(result);
        Assert.NotNull(result.Properties);
        Assert.NotNull(result.Properties.BaseDir);
    }

    [Fact]
    public async Task TestInsertRangeAndDeleteRange()
    {
        var log1 = await GenerateLoggerLog();
        var log2 = await GenerateLoggerLog();
        var log3 = await GenerateLoggerLog();
        var log4 = await GenerateLoggerLog();
        var rows = await _logRsp.InsertRangeAsync([log1, log2, log3, log4]);
        Assert.Equal(4, rows);

        rows =  await _logRsp.DeleteRangeAsync([log1, log2, log3, log4]);
        Assert.Equal(4, rows);
    }


    public async Task TestUpdateAndUpdateRang()
    {
        var log1 = await GenerateLoggerLog();
        var log2 = await GenerateLoggerLog();
        var log3 = await GenerateLoggerLog();
        var log4 = await GenerateLoggerLog();
        var rows = await _logRsp.InsertRangeAsync([log1, log2, log3, log4]);
        Assert.Equal(4, rows);

        log1.Message = "Update";
        rows = await _logRsp.UpdateAsync(log1);
        Assert.Equal(1, rows);

        var newLog1 = await _logRsp.FetchAsync(x => x.Id == log1.Id);   
        Assert.Equal("Update", newLog1.Message);

        log1.Message = "abc";
        log2.Message = "abc";
        log3.Message = "abc";
        log4.Message = "abc";
        rows = await _logRsp.UpdateRangeAsync([log1, log2, log3, log4]);
        Assert.Equal(4, rows);

        var newLog4 = await _logRsp.Where(x => x.Id == log4.Id).FirstOrDefaultAsync();
        Assert.Equal("abc", newLog4.Message);
    }

    private Task<LoggerLog> GenerateLoggerLog()
    {
        var loginLog = new LoggerLog
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Date = DateTime.Now,
            Level = "Info",
            Exception = "Exception",
            Message = "Message",
            Logger = "Logger",
            Properties = new NloglogProperty
            {
                BaseDir = "BaseDir",
                Controller = "Controller",
                ConnectionId = "ConnectionId"
            }
        };

        return Task.FromResult(loginLog);
    }
}
