using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Adnc.UnitTest
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
