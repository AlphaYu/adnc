namespace Adnc.Infra.Unittest.Reposity.TestCases;

public class TransactionTests : IClassFixture<EfCoreDbcontextFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEfRepository<Customer> _customerRsp;
    private readonly IEfRepository<CustomerFinance> _cusFinanceRsp;
    private readonly IEfRepository<CustomerTransactionLog> _custLogsRsp;
    private readonly DbContext _dbContext;
    private readonly EfCoreDbcontextFixture _fixture;

    public TransactionTests(EfCoreDbcontextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _unitOfWork = _fixture.Container.GetRequiredService<IUnitOfWork>();
        _customerRsp = _fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        _cusFinanceRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerFinance>>();
        _custLogsRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();
        _dbContext = _fixture.Container.GetRequiredService<DbContext>();

    }

    private static Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;

    /// <summary>
    /// 测试工作单元(提交)
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestTestUOWCommit()
    {
        var id = UnittestHelper.GetNextId();
        var id2 = UnittestHelper.GetNextId();
        var account = "alpha008";
        var customer = new Customer() { Id = id, Password = "password", Account = account, Nickname = "招财猫", Realname = "张发财" };
        var customer2 = new Customer() { Id = id2, Password = "password", Account = account, Nickname = "招财猫02", Realname = "张发财02" };
        var cusFinance = new CustomerFinance { Account = account, Id = id, Balance = 0 };

        var newNickName = "招财猫008";
        var newRealName = "张发财008";
        var newBalance = 100m;
        try
        {
            _unitOfWork.BeginTransaction();

            // insert efcore
            await _customerRsp.InsertAsync(customer);
            customer2.FinanceInfo = cusFinance;
            await _customerRsp.InsertAsync(customer2);

            //update single
            customer.Nickname = newNickName;
            await _customerRsp.UpdateAsync(customer, UpdatingProps<Customer>(c => c.Nickname));
            var customerFromDb = await _customerRsp.FindAsync(customer.Id);
            Assert.Equal(newNickName, customerFromDb.Nickname);
            Assert.NotEqual(newRealName, customerFromDb.Realname);

            cusFinance.Balance = newBalance;
            await _cusFinanceRsp.UpdateAsync(cusFinance, UpdatingProps<CustomerFinance>(c => c.Balance));
            var financeFromDb = await _customerRsp.FetchAsync(x => x.Id == cusFinance.Id);
            Assert.Equal(newBalance, cusFinance.Balance);

            //update batchs
            await _customerRsp.UpdateRangeAsync(x => x.Id == id, c => new Customer { Realname = newRealName, Nickname = newNickName });

            //delete raw sql
            await _customerRsp.DeleteAsync(id2);

            await _custLogsRsp.DeleteAsync(1000);

            await _custLogsRsp.ExecuteDeleteAsync(x => x.Id == 888888);

            var cusTotal = await _customerRsp.CountAsync(x => x.Id == id || x.Id == id2);
            Assert.Equal(1, cusTotal);

            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            Assert.True(false);
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// 测试工作单元(回滚)
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestUOWRollback()
    {
        var id = UnittestHelper.GetNextId();
        var id2 = UnittestHelper.GetNextId();
        var account = "alpha008";
        var customer = new Customer() { Id = id, Account = account, Nickname = "招财猫", Realname = "张发财" };
        var customer2 = new Customer() { Id = id2, Account = account, Nickname = "招财猫02", Realname = "张发财02" };
        var cusFinance = new CustomerFinance { Account = account, Id = id, Balance = 0 };

        var newNickName = "招财猫008";
        var newRealName = "张发财008";
        var newBalance = 100m;
        try
        {
            _unitOfWork.BeginTransaction();

            // insert efcore
            await _customerRsp.InsertAsync(customer);
            await _customerRsp.InsertAsync(customer2);
            await _cusFinanceRsp.InsertAsync(cusFinance);

            //update single
            customer.Nickname = newNickName;
            await _customerRsp.UpdateAsync(customer, UpdatingProps<Customer>(c => c.Nickname));
            cusFinance.Balance = newBalance;
            await _cusFinanceRsp.UpdateAsync(cusFinance, UpdatingProps<CustomerFinance>(c => c.Balance));

            //update batchs
            await _customerRsp.UpdateRangeAsync(x => x.Id == id, c => new Customer { Realname = newRealName });

            //delete raw sql
            await _customerRsp.DeleteAsync(id2);

            throw new Exception();

#pragma warning disable CS0162 // 检测到无法访问的代码
            _unitOfWork.Commit();
#pragma warning restore CS0162 // 检测到无法访问的代码
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            Assert.True(true);
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        var cusTotal = await _customerRsp.CountAsync(x => x.Id == id || x.Id == id2);
        Assert.Equal(0, cusTotal);

        var customerFromDb = await _customerRsp.FindAsync(id);
        var financeFromDb = await _customerRsp.FetchAsync(x => x.Id == id, c => c.FinanceInfo);

        Assert.Null(customerFromDb);
        Assert.Null(financeFromDb);
    }

    /// <summary>
    ///测试savechanges被事务包裹
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestDbContext()
    {
        var account = "alpha008";

        using var db = await _dbContext.Database.BeginTransactionAsync();

        await _customerRsp.ExecuteDeleteAsync(x => x.Id <= 10000);

        var id = UnittestHelper.GetNextId();
        var customer = new Customer() { Id = id, Password = "password", Account = account, Nickname = "招财猫", Realname = "张发财", FinanceInfo = new CustomerFinance { Id = id, Account = account, Balance = 0 } };

        _dbContext.Add(customer);

        await _dbContext.SaveChangesAsync();

        var id2 = UnittestHelper.GetNextId();
        var customer2 = new Customer() { Id = id2, Password = "password", Account = account, Nickname = "招财猫02", Realname = "张发财02", FinanceInfo = new CustomerFinance { Id = id2, Account = account, Balance = 0 } };

        _dbContext.Add(customer2);

        await _dbContext.SaveChangesAsync();

        await db.CommitAsync();

        Assert.True(true);
    }

    /// <summary>
    /// 测试SaveChanges自动事务
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestAutoTransactions()
    {
        _dbContext.Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;

        var id = UnittestHelper.GetNextId();
        var radmon = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        var cusotmer = new Customer
        {
            Id = id
            ,
            Password = "password"
            ,
            Account = $"a{radmon}"
            ,
            Realname = $"r{radmon}"
            ,
            Nickname = $"n{radmon}"
        };

        await _customerRsp.InsertAsync(cusotmer);

        //不会开启事物
        await _customerRsp.DeleteAsync(id);

        //不会开启事物
        await _customerRsp.ExecuteDeleteAsync(x => x.Id == 10000);

        _dbContext.SaveChanges();
        Assert.True(true);
    }

    [Fact]
    public async Task TestHybirdWriteAndTransaction()
    {
        try
        {
            _unitOfWork.BeginTransaction();

            // insert efcore
            var customer = await InsertCustomer();

            //raw sql find
            var sql = @"SELECT * FROM customerfinance WHERE Id=@Id";
            var dbCusFinance = await _cusFinanceRsp.AdoQuerier.QueryFirstAsync(sql, new { customer.Id }, _cusFinanceRsp.CurrentDbTransaction);
            Assert.NotNull(dbCusFinance);

            var sql2 = @"SELECT * FROM customer ORDER BY Id ASC";
            var dbCustomer = await _customerRsp.AdoQuerier.QueryFirstAsync<Customer>(sql2, null, _customerRsp.CurrentDbTransaction);
            Assert.NotNull(dbCustomer);
            Assert.True(dbCustomer.Id > 0);

            //raw update sql
            var rawSql1 = "update customer set nickname='test8888' where id=1000000000";
            var rows = await _customerRsp.ExecuteSqlRawAsync(rawSql1);
            Assert.True(rows == 0);

            //raw update formatsql
            var newNickName = "test8888";
            FormattableString formatSql2 = $"update customer set nickname={newNickName} where id={dbCustomer.Id}";
            rows = await _customerRsp.ExecuteSqlInterpolatedAsync(formatSql2);
            Assert.True(rows > 0);

            await _customerRsp.DeleteAsync(1);

            await _customerRsp.ExecuteDeleteAsync(x => x.Id <=10000);

            //ef search
            var dbCustomer2 = await _customerRsp.FindAsync(dbCustomer.Id, x => x.FinanceInfo);
            Assert.NotNull(dbCustomer2);
            Assert.Equal("test8888", dbCustomer2.Nickname);

            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            Assert.True(false); 
            throw new Exception(ex.Message, ex);
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// 生成测试数据
    /// </summary>
    /// <returns></returns>
    private async Task<Customer> InsertCustomer()
    {
        var id = UnittestHelper.GetNextId();
        var customer = new Customer() { Id = id, Password = "password", Account = "alpha2008", Nickname = UnittestHelper.GetNextId().ToString(), Realname = UnittestHelper.GetNextId().ToString() };
        customer.FinanceInfo = new CustomerFinance { Id = customer.Id, Account = customer.Account, Balance = 0 };
        await _customerRsp.InsertAsync(customer);
        return customer;
    }
}
