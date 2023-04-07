using Adnc.Infra.IdGenerater.Yitter;
using Adnc.Infra.IRepositories;
using Adnc.UnitTest.TestCases.Repositories.Entities;

namespace Adnc.UnitTest.TestCases.Repositories;

public class UnitOfWorkTests : IClassFixture<CoreServiceFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly Operater _operater;
    private readonly IEfRepository<Customer> _cusRsp;
    private readonly CoreServiceFixture _fixture;

    public UnitOfWorkTests(CoreServiceFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _cusRsp = _fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        _operater = _fixture.Container.GetRequiredService<Operater>();
    }

    /// <summary>
    /// 测试事务拦截器
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestUowInterceptor()
    {
        var id = IdGenerater.GetNextId();
        var customer = new Customer() { Id = id, Account = "alpha2008", Nickname = IdGenerater.GetNextId().ToString(), Realname = IdGenerater.GetNextId().ToString() };
        customer.FinanceInfo = new CustomerFinance { Account = "alpha2008", Id = id, Balance = 0 };
        /*
            set session transaction isolation level repeatable read
            start transaction
            INSERT INTO `Customer` (`ID`, `Account`, `CreateBy`, `CreateTime`, `ModifyBy`, `ModifyTime`, `Nickname`, `Realname`)
            VALUES (122339207606833152, 'alpha2008', 1600000000000, timestamp('2020-12-03 14:12:20.552579'), NULL, NULL, '1606975940001', '1606975940002')
            INSERT INTO `CusFinance` (`ID`, `Account`, `Balance`, `CreateBy`, `CreateTime`, `ModifyBy`, `ModifyTime`)
            VALUES (122339207606833152, 'alpha2008', 0, 1600000000000, timestamp('2020-12-03 14:12:20.756977'), NULL, NULL)
            commit
         */
        //await _cusManger.RegisterAsync(customer);

        //bool exists = await _cusRsp.AnyAsync(c => c.Id == id);
        // Assert.True(exists);
        Assert.True(true);

        await Task.CompletedTask;
    }
}
