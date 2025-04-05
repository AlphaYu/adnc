namespace Adnc.Infra.Unittest.Reposity.Fixtures;

internal class UnittestHelper
{
    public static long GetNextId()
    {
        Task.Delay(2).Wait();
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }
}
