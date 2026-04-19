namespace Adnc.Infra.Unittest.Reposity.TestCases;

public class MaxscaleTests : IClassFixture<EfCoreDbcontextFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEfRepository<Customer> _cusRsp;
    private readonly IEfRepository<CustomerFinance> _cusFinanceRsp;
    private readonly IEfRepository<CustomerTransactionLog> _cusLogsRsp;
    private readonly EfCoreDbcontextFixture _fixture;

    public MaxscaleTests(EfCoreDbcontextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _unitOfWork = _fixture.Container.GetRequiredService<IUnitOfWork>();
        _cusRsp = _fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        _cusFinanceRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerFinance>>();
        _cusLogsRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();
    }

    [Fact]
    public async Task TestReadFromWirteDb()
    {
        var result = await InsertCustomer();
        var cusFinance = await InsertCusFinance(result.Id);

        var cus = await _cusRsp.FetchAsync(c => c.Id == result.Id, writeDb: true);
        Assert.NotNull(cus);

        await _cusFinanceRsp.DeleteAsync(result.Id);
        await _cusRsp.DeleteAsync(result.Id);
    }

    [Fact]
    public async Task TestDapperReadFromWirteDb()
    {
        var result = await InsertCustomer();
        var cusFinance = await InsertCusFinance(result.Id);

        var cus = await _cusRsp.AdoQuerier.QueryAsync<Customer>("select * FROM customer where id=@id", new { id = result.Id }, writeDb: true);
        Assert.NotNull(cus);

        await _cusFinanceRsp.DeleteAsync(result.Id);
        await _cusRsp.DeleteAsync(result.Id);
    }

    [Fact]
    public async Task TestInsertRange()
    {
        var list = await InsertRangeCustomer(10);
        var ids = list.Select(c => c.Id).ToArray();
        var cusCount = await _cusRsp.CountAsync(c => ids.Contains(c.Id), writeDb: true);
        var cusFinanceCount = await _cusFinanceRsp.CountAsync(c => ids.Contains(c.Id), writeDb: true);

        Assert.Equal(10, cusCount);
        Assert.Equal(10, cusFinanceCount);

        await _cusFinanceRsp.ExecuteDeleteAsync(c => ids.Contains(c.Id));
        await _cusRsp.ExecuteDeleteAsync(c => ids.Contains(c.Id));
    }

    [Fact]
    public async Task TestUpdate()
    {
        var newRealName = "TestUser";
        var newNickname = "Test";
        long id = 0;

        try
        {
            _unitOfWork.BeginTransaction();

            var result = await InsertCustomer();
            id = result.Id;

            await _cusRsp.ExecuteUpdateAsync(c => c.Id == result.Id,
                setters => setters
                    .SetProperty(c => c.Realname, newRealName)
                    .SetProperty(c => c.Nickname, newNickname));

            _unitOfWork.Commit();
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        var newCus = await _cusRsp.FetchAsync(c => c.Id == id, writeDb: true);
        Assert.Equal(newRealName, newCus.Realname);
        Assert.Equal(newNickname, newCus.Nickname);
    }

    [Fact]
    public async Task TestUpdateFullColumn()
    {
        var newRealName = "TestUser";
        var newNickname = "Test";
        long id = 0;

        try
        {
            _unitOfWork.BeginTransaction();

            var result = await InsertCustomer();
            id = result.Id;

            result.Realname = newRealName;
            result.Nickname = newNickname;
            await _cusRsp.UpdateAsync(result);

            _unitOfWork.Commit();
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        var newCus = await _cusRsp.FetchAsync(c => c.Id == id, writeDb: true);
        Assert.Equal(newRealName, newCus.Realname);
        Assert.Equal(newNickname, newCus.Nickname);
    }

    [Fact]
    public async Task TestUpdateRange()
    {
        var list = await InsertRangeCustomer(10);
        var ids = list.Select(c => c.Id).ToArray();
        var newRealName = "TestUser001";
        var newNickname = "Test";

        await _cusRsp.ExecuteUpdateAsync(c => ids.Contains(c.Id),
            setters => setters
                .SetProperty(c => c.Realname, newRealName)
                .SetProperty(c => c.Nickname, newNickname));

        var newCus = await _cusRsp.FetchAsync(c => c.Id == list[0].Id, writeDb: true);

        Assert.Equal(newRealName, newCus.Realname);
        Assert.Equal(newNickname, newCus.Nickname);

        await _cusFinanceRsp.ExecuteDeleteAsync(c => ids.Contains(c.Id));
        await _cusRsp.ExecuteDeleteAsync(c => ids.Contains(c.Id));
    }

    /// <summary>
    /// Generate test data
    /// </summary>
    /// <returns></returns>
    private async Task<List<Customer>> InsertRangeCustomer(int rows)
    {
        var list = new List<Customer>();
        for (int i = 0; i < rows; i++)
        {
            var id = UnittestHelper.GetNextId();
            var customer = new Customer() { Id = id, Account = "alpha2008", Nickname = UnittestHelper.GetNextId().ToString(), Realname = UnittestHelper.GetNextId().ToString() };
            customer.FinanceInfo = new CustomerFinance { Account = "alpha2008", Id = id, Balance = 0 };
            list.Add(customer);
        }
        await _cusRsp.InsertRangeAsync(list);

        return list;
    }

    /// <summary>
    /// Generate test data
    /// </summary>
    /// <returns></returns>
    private async Task<Customer> InsertCustomer()
    {
        var id = UnittestHelper.GetNextId();
        var customer = new Customer() { Id = id, Account = "alpha2008", Password = "password", Nickname = UnittestHelper.GetNextId().ToString(), Realname = UnittestHelper.GetNextId().ToString() };
        await _cusRsp.InsertAsync(customer);
        return customer;
    }

    /// <summary>
    /// Generate test data
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<CustomerFinance> InsertCusFinance(long id)
    {
        var cusFinance = new CustomerFinance { Account = "alpha2008", Id = id, Balance = 0 };
        await _cusFinanceRsp.InsertAsync(cusFinance);
        return cusFinance;
    }
}
