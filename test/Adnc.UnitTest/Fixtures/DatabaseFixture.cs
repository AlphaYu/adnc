using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Adnc.UnitTest.Fixtures
{

    //针对 Scoped 的对象可以借助 XUnit 中的 IClassFixture 来实现
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Db = new SqlConnection("MyConnectionString");
            // ... initialize data in the test database ...	
        }
        public void Dispose()
        {
            // ... clean up test data from the database ...	
        }
        public SqlConnection Db { get; private set; }
    }
}
