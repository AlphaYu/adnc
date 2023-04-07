using Adnc.Infra.IdGenerater.Yitter;
using Adnc.Infra.IRepositories;
using Adnc.UnitTest.TestCases.Repositories.Entities;

namespace Adnc.UnitTest.TestCases.Repositories;

public class MaxscaleTests : IClassFixture<MaxscaleDbcontextFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEfRepository<Customer> _cusRsp;
    private readonly IEfRepository<CustomerFinance> _cusFinanceRsp;
    private readonly IEfRepository<CustomerTransactionLog> _cusLogsRsp;
    private readonly MaxscaleDbcontextFixture _fixture;

    public MaxscaleTests(MaxscaleDbcontextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _unitOfWork = _fixture.Container.GetRequiredService<IUnitOfWork>();
        _cusRsp = _fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        _cusFinanceRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerFinance>>();
        _cusLogsRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();
        if (IdGenerater.CurrentWorkerId < 0)
            IdGenerater.SetWorkerId(1);
    }

    protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;

    [Fact]
    public async Task TestReadFromWirteDb()
    {
        var result = await InsertCustomer();
        var cusFinance = await InsertCusFinance(result.Id);

        var cus = await _cusRsp.FetchAsync(c => c, c => c.Id == result.Id, writeDb: true);
        Assert.NotNull(cus);

        await _cusFinanceRsp.DeleteAsync(result.Id);
        await _cusRsp.DeleteAsync(result.Id);
    }

    [Fact]
    public async Task TestDapperReadFromWirteDb()
    {
        var result = await InsertCustomer();
        var cusFinance = await InsertCusFinance(result.Id);

        var cus = await _cusRsp.AdoQuerier.QueryAsync<Customer>("select * from customer where id=@id", new { id = result.Id }, writeDb: true);
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

        await _cusFinanceRsp.DeleteRangeAsync(c => ids.Contains(c.Id));
        await _cusRsp.DeleteRangeAsync(c => ids.Contains(c.Id));
    }

    [Fact]
    public async Task TestUpdate()
    {
        var newRealName = "测试用户";
        var newNickname = "测试";
        long id = 0;

        try
        {
            _unitOfWork.BeginTransaction();

            var result = await InsertCustomer();
            id = result.Id;

            result.Realname = newRealName;
            result.Nickname = newNickname;
            await _cusRsp.UpdateAsync(result, UpdatingProps<Customer>(c => c.Realname, c => c.Nickname));

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

        var newCus = await _cusRsp.FindAsync(id, writeDb: true);
        Assert.Equal(newRealName, newCus.Realname);
        Assert.Equal(newNickname, newCus.Nickname);
    }

    [Fact]
    public async Task TestUpdateFullColumn()
    {
        var newRealName = "测试用户";
        var newNickname = "测试";
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

        var newCus = await _cusRsp.FindAsync(id, writeDb: true);
        Assert.Equal(newRealName, newCus.Realname);
        Assert.Equal(newNickname, newCus.Nickname);
    }

    [Fact]
    public async Task TestUpdateRange()
    {
        var list = await InsertRangeCustomer(10);
        var ids = list.Select(c => c.Id).ToArray();
        var newRealName = "测试用户001";
        var newNickname = "测试";

        await _cusRsp.UpdateRangeAsync(c => ids.Contains(c.Id), c => new Customer { Realname = newRealName, Nickname = newNickname });

        var newCus = await _cusRsp.FindAsync(list[0].Id, writeDb: true);

        Assert.Equal(newRealName, newCus.Realname);
        Assert.Equal(newNickname, newCus.Nickname);

        await _cusFinanceRsp.DeleteRangeAsync(c => ids.Contains(c.Id));
        await _cusRsp.DeleteRangeAsync(c => ids.Contains(c.Id));
    }

    /// <summary>
    /// 生成测试数据
    /// </summary>
    /// <returns></returns>
    private async Task<List<Customer>> InsertRangeCustomer(int rows)
    {
        var list = new List<Customer>();
        for (int i = 0; i < rows; i++)
        {
            var id = IdGenerater.GetNextId();
            var customer = new Customer() { Id = id, Account = "alpha2008", Nickname = IdGenerater.GetNextId().ToString(), Realname = IdGenerater.GetNextId().ToString() };
            customer.FinanceInfo = new CustomerFinance { Account = "alpha2008", Id = id, Balance = 0 };
            list.Add(customer);
        }
        await _cusRsp.InsertRangeAsync(list);

        return list;
    }

    /// <summary>
    /// 生成测试数据
    /// </summary>
    /// <returns></returns>
    private async Task<Customer> InsertCustomer()
    {
        var id = IdGenerater.GetNextId();
        var customer = new Customer() { Id = id, Account = "alpha2008", Nickname = IdGenerater.GetNextId().ToString(), Realname = IdGenerater.GetNextId().ToString() };
        await _cusRsp.InsertAsync(customer);
        return customer;
    }

    /// <summary>
    /// 生成测试数据
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
