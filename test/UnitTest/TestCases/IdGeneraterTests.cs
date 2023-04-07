using Adnc.Infra.IdGenerater.Yitter;

namespace Adnc.UnitTest.TestCases;

public class IdGeneraterTests
{
    private readonly ITestOutputHelper _output;

    public IdGeneraterTests(ITestOutputHelper output)
    {
        _output = output;
        IdGenerater.SetWorkerId(1);
    }

    /// <summary>
    /// id 小于 js 最大值 151599672553541
    /// </summary>
    [Fact]
    public void TestIdLessThanjJsMaxNumber()
    {
        long id = IdGenerater.GetNextId();
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
            set[index - 1] = IdGenerater.GetNextId();
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
            IdGenerater.GetNextId();
        });
        stopwatch.Stop();
        //持续时间: 517 毫秒
        _output.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
        stopwatch.Reset();
    }
}