namespace Adnc.Infra.Unittest.Redis.TestCases;

public class IdGeneraterTests : IClassFixture<RedisContextFixture>
{
    private static bool _isSet =false;
    private readonly ITestOutputHelper _output;

    public IdGeneraterTests(ITestOutputHelper output)
    {
        _output = output;
        if (!_isSet)
        {
            _isSet = true;
            IdGenerater.Yitter.IdGenerater.SetWorkerId(1);
        }
    }

    /// <summary>
    /// The ID is smaller than the JavaScript max safe integer 9007199254740992
    /// </summary>
    [Fact]
    public void TestIdLessThanjJsMaxNumber()
    {
        long id = IdGenerater.Yitter.IdGenerater.GetNextId();
        _output.WriteLine(id.ToString());
        Assert.True(9007199254740992 > id);
    }

    /// <summary>
    /// 100k IDs with no duplicates
    /// </summary>
    [Fact]
    public void TestNotContainsDuplicateIds()
    {
        var set = new long[100000];
        Parallel.For(1, 100000, index =>
        {
            set[index - 1] = IdGenerater.Yitter.IdGenerater.GetNextId();
        });
        Assert.Equal(100000, set.Distinct().Count());
    }

    /// <summary>
    /// 100k IDs in 517 milliseconds
    /// </summary>
    [Fact]
    public void TestSpeed()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        Parallel.For(1, 100000, index =>
        {
            IdGenerater.Yitter.IdGenerater.GetNextId();
        });
        stopwatch.Stop();
        // Duration: 517 milliseconds
        _output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
        stopwatch.Reset();
    }
}
