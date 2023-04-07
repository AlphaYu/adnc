using Xunit;
using Adnc.UnitTest.Fixtures;

namespace Adnc.UnitTest
{

    //针对 Scoped 的对象可以借助 XUnit 中的 IClassFixture 来实现
    public class MyDatabaseTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture _fixture;
        public MyDatabaseTests(DatabaseFixture fixture)
        {
            this._fixture = fixture;
        }
        //[Fact]
        //public async Task GetTest()
        //{
        //    // ... write tests, using fixture.Db to get access to the SQL Server ...	
        //    // ... 在这里使用注入 的 DatabaseFixture	
        //    var db = _fixture.Db;
        //}

    }
}
