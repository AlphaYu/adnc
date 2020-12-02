using Xunit.Abstractions;

namespace Adnc.UnitTest.Base
{
    public class TestBase
    {
        protected readonly ITestOutputHelper Output;

        public TestBase(ITestOutputHelper tempOutput)
        {
            Output = tempOutput;
        }
    }
}
