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
    /// id 小于 js 最大值 9007199254740992
    /// </summary>
    [Fact]
    public void TestIdLessThanjJsMaxNumber()
    {
        long id = IdGenerater.Yitter.IdGenerater.GetNextId();
        _output.WriteLine(id.ToString());
        Assert.True(9007199254740992 > id);
    }

    /// <summary>
    /// 10w个ids,没有重复
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
    /// 10W个ids,517毫秒
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
        //持续时间: 517 毫秒
        _output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
        stopwatch.Reset();
    }
}
