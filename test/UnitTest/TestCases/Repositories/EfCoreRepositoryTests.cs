using Adnc.Infra.IdGenerater.Yitter;
using Adnc.Infra.IRepositories;
using Adnc.UnitTest.TestCases.Repositories.Entities;

namespace Adnc.UnitTest.TestCases.Repositories;

public class EfCoreRepositoryTests : IClassFixture<EfCoreDbcontextFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEfRepository<Customer> _customerRsp;
    private readonly IEfRepository<CustomerFinance> _cusFinanceRsp;
    private readonly IEfRepository<CustomerTransactionLog> _custLogsRsp;
    private readonly DbContext _dbContext;
    private readonly EfCoreDbcontextFixture _fixture;

    public EfCoreRepositoryTests(EfCoreDbcontextFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
        _unitOfWork = _fixture.Container.GetRequiredService<IUnitOfWork>();
        _customerRsp = _fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        _cusFinanceRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerFinance>>();
        _custLogsRsp = _fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();
        _dbContext = _fixture.Container.GetRequiredService<DbContext>();
        if (IdGenerater.CurrentWorkerId < 0)
            IdGenerater.SetWorkerId(1);
    }

    private static Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;

    /// <summary>
    /// 插入
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestInsert()
    {
        var id = IdGenerater.GetNextId();
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
            ,
            FinanceInfo = new CustomerFinance { Id = id, Account = $"a{radmon}", Balance = 0 }
        };

        await _customerRsp.InsertAsync(cusotmer);

        var newCust = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT *  FROM Customer WHERE Id=@Id", new { Id = id });
        Assert.NotEmpty(newCust);

        var newCustAccounts = await _customerRsp.AdoQuerier.QueryAsync<string>("SELECT Account  FROM CustomerFinance WHERE Account=@account", new { account = newCust.First().Account });
        Assert.Equal(newCust.First().Account, newCustAccounts.First());
    }

    /// <summary>
    /// 批量插入
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestInsertRange()
    {
        var customer = await _customerRsp.FetchAsync(x => x, x => x.Id > 1);

        var id0 = IdGenerater.GetNextId();
        var id1 = IdGenerater.GetNextId();
        var logs = new List<CustomerTransactionLog>
        {
            new CustomerTransactionLog{ Id=id0,Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeBehavior.Recharge,ExchageStatus=ExchageStatus.Finished,Remark="test"}
            ,
            new CustomerTransactionLog{ Id=id1,Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeBehavior.Recharge,ExchageStatus=ExchageStatus.Finished,Remark="test"}
        };

        await _custLogsRsp.InsertRangeAsync(logs);

        var logsFromDb = await _custLogsRsp.Where(x => x.Id == id0 || x.Id == id1).ToListAsync();
        Assert.NotEmpty(logsFromDb);
        Assert.Equal(2, logsFromDb.Count);
    }

    /// <summary>
    /// 测试更新
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestUpdateWithTraking()
    {
        var ids = await _customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM Customer ORDER BY ID ASC LIMIT 0,2");
        var id0 = ids.ToArray()[0];

        //IEfRepository<>默认关闭了跟踪，需要手动开启跟踪
        var customer = await _customerRsp.FindAsync(id0, noTracking: false);
        //实体已经被跟踪
        customer.Realname = "被跟踪01";
        await _customerRsp.UpdateAsync(customer);
        var newCust1 = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE Id=@Id", new { Id = id0 });
        Assert.Equal("被跟踪01", newCust1.FirstOrDefault().Realname);

        var customerId = (await _customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id  FROM CustomerFinance limit 0,1")).FirstOrDefault();
        customer = await _customerRsp.FindAsync(x => x.Id == customerId, x => x.FinanceInfo, noTracking: false);
        customer.Account = "主从更新01";
        customer.FinanceInfo.Account = "主从更新01";
        await _customerRsp.UpdateAsync(customer);
        var newCust2 = await _customerRsp.FindAsync(customerId, x => x.FinanceInfo);
        Assert.Equal("主从更新01", newCust2.Account);
        Assert.Equal("主从更新01", newCust2.FinanceInfo.Account);
    }

    /// <summary>
    /// 更新，指定列
    /// </summary>
    [Fact]
    public async Task TestUpdateAssigns()
    {
        var ids = await _customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM Customer ORDER BY ID ASC LIMIT 0,4");
        var id0 = ids.ToArray()[0];
        var customer = await _customerRsp.FindAsync(id0, noTracking: false);
        //实体已经被跟踪并且指定更新列,     更新列没有指定Realname，该列不会被更新
        customer.Nickname = "更新指定列";
        customer.Realname = "不指定该列";
        await _customerRsp.UpdateAsync(customer, UpdatingProps<Customer>(c => c.Nickname));
        var newCus = (await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@ID", customer)).FirstOrDefault();
        Assert.Equal("更新指定列", newCus.Nickname);
        Assert.NotEqual("不指定该列", newCus.Realname);

        //实体没有被跟踪，dbcontext中有没有同名实体
        var id1 = ids.ToArray()[1];
        var customer1 = await _customerRsp.FindAsync(id1, noTracking: true);
        customer1.Account = "adnc-new";
        customer1.Realname = "没被跟踪01";
        customer1.Nickname = "新昵称01";
        await _customerRsp.UpdateAsync(customer1, UpdatingProps<Customer>(c => c.Realname, c => c.Nickname));
        var newCus1 = (await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@ID", new { Id = id1 })).FirstOrDefault();
        Assert.Equal("没被跟踪01", newCus1.Realname);
        Assert.Equal("新昵称01", newCus1.Nickname);
        Assert.NotEqual(customer1.Account, newCus1.Account);

        //实体没有被跟踪，dbcontext中有没有同名实体
        var id2 = ids.ToArray()[2];
        await _customerRsp.UpdateAsync(new Customer { Id = id2, Realname = "没被跟踪02", Nickname = "新昵称02" }, UpdatingProps<Customer>(c => c.Realname, c => c.Nickname));
        var newCus2 = await _customerRsp.FindAsync(id2);
        Assert.Equal("没被跟踪02", newCus2.Realname);
        Assert.Equal("新昵称02", newCus2.Nickname);
        Assert.True(newCus2.Account.IsNotNullOrWhiteSpace());

        //实体没有被跟踪,dbcontext中有有同名实体
        var customer3 = await InsertCustomer();
        customer3.Account = "adnc-3";
        customer3.Realname = "没被跟踪03";
        customer3.Nickname = "新昵称03";
        await _customerRsp.UpdateAsync(customer3, UpdatingProps<Customer>(c => c.Realname, c => c.Nickname));
        var newCus3 = await _customerRsp.FindAsync(customer3.Id);
        Assert.Equal("没被跟踪03", newCus3.Realname);
        Assert.Equal("新昵称03", newCus3.Nickname);
        Assert.True(newCus3.Account.IsNotNullOrWhiteSpace());
    }

    /// <summary>
    /// 批量更新，指定列
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestUpdateRange()
    {
        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();
        var total = await _customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
        Assert.Equal(2, total);

        await _customerRsp.UpdateRangeAsync(c => c.Id == cus1.Id || c.Id == cus2.Id, x => new Customer { Realname = "批量更新" });
        var result2 = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
        Assert.NotEmpty(result2);
        Assert.Equal("批量更新", result2.FirstOrDefault().Realname);
        Assert.Equal("批量更新", result2.LastOrDefault().Realname);
    }

    /// <summary>
    /// 批量更新，带跟踪
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestUpdateRangeWithTrack()
    {
        var customers = await _customerRsp.GetAll(noTracking: false).Skip(0).Take(2).ToListAsync();
        foreach (var customer in customers)
        {
            customer.Realname = "RangeWithTrack";
        }
        await _customerRsp.UpdateRangeAsync(customers);
        var result2 = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID in @ids", new { ids = customers.Select(x=>x.Id) });
        Assert.NotEmpty(result2);
        Assert.Equal("RangeWithTrack", result2.FirstOrDefault().Realname);
        Assert.Equal("RangeWithTrack", result2.LastOrDefault().Realname);

        var customers2 = await _customerRsp.GetAll(noTracking: false).Skip(0).Take(2).ToListAsync();
        foreach (var customer in customers2)
        {
            customer.Realname = "RangeWithNoTrack";
        }
        try
        {
            int rows = await _customerRsp.UpdateRangeAsync(customers2);
            Assert.False(rows > 0);
        }
        catch
        {
            _output.WriteLine("不允许更新");
        }
    }

    [Fact]
    public async Task TestUpdateRangeDifferentMembers()
    {
        var cus1 = await InsertCustomer();
        var cus2 = await _customerRsp.FindAsync(x => x.Id < cus1.Id, navigationPropertyPath: null, x => x.Id, false, noTracking: false);
        cus2.Nickname = "string";
        var cus3 = await _customerRsp.FindAsync(x => x.Id < cus2.Id);
        var cus4 = (await _customerRsp.AdoQuerier.QueryAsync<Customer>($"SELECT * FROM Customer  WHERE ID<{cus3.Id} ORDER BY ID ASC  LIMIT 0,1")).FirstOrDefault();

        var propertyNameAndValues = new Dictionary<long, List<(string propertyName, dynamic propertyValue)>>
        {
            {
                cus1.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",IdGenerater.GetNextId().ToString()),
                   ("Nickname","Nickname1111")
                }
            },
            {
                cus2.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",IdGenerater.GetNextId().ToString()),
                   ("Nickname","Nickname2222")
                }
            },
            {
                cus3.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",IdGenerater.GetNextId().ToString()),
                   ("Nickname","Nickname3333")
                }
            }
            ,
            {
                cus4.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",IdGenerater.GetNextId().ToString()),
                   ("Nickname","Nickname4444")
                }
            }
        };

        var total = await _customerRsp.UpdateRangeAsync(propertyNameAndValues);

        _output.WriteLine(total.ToString());

        Assert.Equal(4, total);
    }

    /// <summary>3
    /// 测试删除
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestDelete()
    {
        //删除，无跟踪状态
        var customer = await InsertCustomer();
        var customerFromDb = await _customerRsp.FindAsync(customer.Id);
        Assert.Equal(customer.Id, customerFromDb.Id);

        await _customerRsp.DeleteAsync(customer.Id);
        var result = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@Id", new { customer.Id });
        Assert.Null(result);

        //删除，有跟踪状态
        var customer2 = await InsertCustomer();
        await _customerRsp.DeleteAsync(customer2.Id);
        result = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@Id", new { customer2.Id });
        Assert.Null(result);

        var ids = await _customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM Customer ORDER BY ID DESC LIMIT 1");
        //删除，上下文中无该实体。
        var id = ids.First();
        await _customerRsp.DeleteAsync(id);
        result = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@Id", new { Id = id });
        Assert.Null(result);

        //删除不存在的记录
        try
        {
            await _customerRsp.DeleteAsync(99872221111111111);
        }
        catch (Exception ex)
        {
            Assert.True(ex is DbUpdateConcurrencyException);
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestDeleteRange()
    {
        //batch hand delete
        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();
        var total = await _customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
        Assert.Equal(2, total);

        await _customerRsp.DeleteRangeAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
        var result2 = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
        Assert.Null(result2);
    }

    /// <summary>
    /// 测试条件查询单条记录
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestFetch()
    {
        //指定列查询
        var customer = await _customerRsp.FetchAsync(x => new { x.Id, x.Account }, x => x.Id > 1);
        Assert.NotNull(customer);

        //指定列查询，指定列包含导航属性
        var customer2 = await _customerRsp.FetchAsync(x => new { x.Id, x.Account, x.FinanceInfo }, x => x.Id > 1);
        Assert.NotNull(customer2);

        //不指定列查询
        var customer3 = await _customerRsp.FindAsync(x => x.Id > 1);
        Assert.NotNull(customer3);

        //不指定列查询，预加载导航属性
        var customer4 = await _customerRsp.FindAsync(x => x.Id > 1, x => x.FinanceInfo);
        Assert.NotNull(customer4);
    }

    /// <summary>
    /// 测试ID查询
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestFind()
    {
        var customerId = (await _customerRsp.AdoQuerier.QueryAsync<long>("SELECT CustomerId FROM CustomerTransactionLog ORDER BY ID DESC LIMIT 0,1")).FirstOrDefault();

        //不加载导航属性
        var customer3 = await _customerRsp.FindAsync(customerId);
        Assert.NotNull(customer3);
        Assert.Null(customer3.FinanceInfo);

        //加载导航属性
        var customer4 = await _customerRsp.FindAsync(customerId, x => x.TransactionLogs);
        Assert.NotNull(customer4);
        Assert.NotEmpty(customer4.TransactionLogs);
    }

    /// <summary>
    /// 测试where and getall
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestWhereAndGetAll()
    {
        var customers = await _customerRsp.Where(x => x.Id > 1).ToListAsync();
        Assert.NotEmpty(customers);

        var customer = await _customerRsp.Where(x => x.Id > 1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        Assert.NotNull(customer);

        //GetAll() = Where(x=>true)
        var customerAll = _customerRsp.GetAll();
        var custsLogs = _custLogsRsp.GetAll();

        var logs = await customerAll.Join(custsLogs, c => c.Id, t => t.CustomerId, (c, t) => new
        {
            t.Id
            ,
            t.CustomerId
            ,
            t.Account
            ,
            t.ChangedAmount
            ,
            t.ChangingAmount
            ,
            c.Realname
        })
        .Where(c => c.Id > 1)
        .ToListAsync();

        Assert.NotEmpty(logs);
    }

    /// <summary>
    /// 测试sql查询
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestQuery()
    {
        var sql = $@"SELECT `c0`.`Id`, `c0`.`CustomerId`, `c0`.`Account`, `c0`.`ChangedAmount`, `c0`.`ChangingAmount`, `c`.`Realname`
                        FROM `Customer` AS `c`
                        INNER JOIN `CustomerTransactionLog` AS `c0` ON `c`.`Id` = `c0`.`CustomerId`
                        WHERE `c0`.`Id` > @Id";
        var logs = await _customerRsp.AdoQuerier.QueryAsync(sql, new { Id = 1 });

        Assert.NotEmpty(logs);
    }

    /// <summary>
    /// 测试多种查询
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestReutrnNullResult()
    {
        var result = await _customerRsp.FindAsync(x => x.Id == 999);
        Assert.Null(result);

        result = await _customerRsp.FindAsync(999);
        Assert.Null(result);

        result = await _customerRsp.Where(x => x.Id == 999).FirstOrDefaultAsync();
        Assert.Null(result);

        var dapperResult = await _customerRsp.AdoQuerier.QueryAsync<Customer>("select * from Customer where id=999");
        Assert.Null(dapperResult);

        dynamic dapperResult2 = await _customerRsp.AdoQuerier.QueryAsync("select * from Customer where id=999");
        Assert.Null(dapperResult2);
    }

    /// <summary>
    /// 测试工作单元(提交)
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestTestUOWCommit()
    {
        var id = IdGenerater.GetNextId();
        var id2 = IdGenerater.GetNextId();
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
            var financeFromDb = await _customerRsp.FetchAsync(c => c, x => x.Id == cusFinance.Id);
            Assert.Equal(newBalance, cusFinance.Balance);

            //update batchs
            await _customerRsp.UpdateRangeAsync(x => x.Id == id, c => new Customer { Realname = newRealName, Nickname = newNickName });

            //delete raw sql
            await _customerRsp.DeleteAsync(id2);

            await _custLogsRsp.DeleteAsync(1000);

            var cusTotal = await _customerRsp.CountAsync(x => x.Id == id || x.Id == id2);
            Assert.Equal(1, cusTotal);

            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
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
        var id = IdGenerater.GetNextId();
        var id2 = IdGenerater.GetNextId();
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
        }
        finally
        {
            _unitOfWork.Dispose();
        }

        var cusTotal = await _customerRsp.CountAsync(x => x.Id == id || x.Id == id2);
        Assert.Equal(0, cusTotal);

        var customerFromDb = await _customerRsp.FindAsync(id);
        var financeFromDb = await _customerRsp.FetchAsync(c => c, x => x.Id == id);

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

        await _customerRsp.DeleteRangeAsync(x => x.Id <= 10000);

        var id = IdGenerater.GetNextId();
        var customer = new Customer() { Id = id, Password = "password", Account = account, Nickname = "招财猫", Realname = "张发财", FinanceInfo = new CustomerFinance { Id = id, Account = account, Balance = 0 } };

        _dbContext.Add(customer);

        await _dbContext.SaveChangesAsync();

        var id2 = IdGenerater.GetNextId();
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
        var defaultAutoTransaction = _dbContext.Database.AutoTransactionsEnabled;
        if (!defaultAutoTransaction)
            _dbContext.Database.AutoTransactionsEnabled = true;

        var id = IdGenerater.GetNextId();
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

        //受自动事务控制
        await _customerRsp.DeleteAsync(id);

        //不受自动事务控制
        await _customerRsp.DeleteRangeAsync(x => x.Id == 10000);

        _dbContext.SaveChanges();
        _dbContext.Database.AutoTransactionsEnabled = false;
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
            var sql = @"SELECT * FROM CustomerFinance WHERE Id=@Id";
            var dbCusFinance = await _cusFinanceRsp.AdoQuerier.QueryFirstAsync(sql, new { customer.Id }, _cusFinanceRsp.CurrentDbTransaction);
            Assert.NotNull(dbCusFinance);

            var sql2 = @"SELECT * FROM Customer ORDER BY Id ASC";
            var dbCustomer = await _customerRsp.AdoQuerier.QueryFirstAsync<Customer>(sql2, null, _customerRsp.CurrentDbTransaction);
            Assert.NotNull(dbCustomer);
            Assert.True(dbCustomer.Id > 0);

            //raw update sql
            var rawSql1 = "update Customer set nickname='test8888' where id=1000000000";
            var rows = await _customerRsp.ExecuteSqlRawAsync(rawSql1);
            Assert.True(rows == 0);

            //raw update formatsql
            var newNickName = "test8888";
            FormattableString formatSql2 = $"update Customer set nickname={newNickName} where id={dbCustomer.Id}";
            rows = await _customerRsp.ExecuteSqlInterpolatedAsync(formatSql2);
            Assert.True(rows > 0);

            //ef search
            var dbCustomer2 = await _customerRsp.FindAsync(dbCustomer.Id, x => x.FinanceInfo);
            Assert.NotNull(dbCustomer2?.FinanceInfo);
            Assert.Equal("test8888", dbCustomer2.Nickname);

            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
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
        var id = IdGenerater.GetNextId();
        var customer = new Customer() { Id = id, Password = "password", Account = "alpha2008", Nickname = IdGenerater.GetNextId().ToString(), Realname = IdGenerater.GetNextId().ToString() };
        customer.FinanceInfo = new CustomerFinance { Id = customer.Id, Account = customer.Account, Balance = 0 };
        await _customerRsp.InsertAsync(customer);
        return customer;
    }

    #region old testing codes

    //[Fact]
    //public async Task TestConcurrencyCheck()
    //{
    //    //var user1 = await _userRepository.FetchAsync(x => x.ID == 1);
    //    //user1.Email = "1alphacn@foxmail.com";
    //    //user1.Sex = 2;
    //    //UPDATE `SysUser` SET `Email` = @p0, `ModifyTime` = @p1, `Sex` = @p2
    //    //WHERE `ID` = @p3 AND `RowVersion` = @p4;
    //    //var result2 = await _userRepository.UpdateAsync(user1, u => u.Email, u => u.Sex);

    // var finance = new SysUserFinance { ID = 4, Amount = 450, RowVersion =
    // DateTime.Parse("2020-07-01 21:51:12.310") };

    // //单实体更新(指定列)，有效。 //var reuslt = await _userFinanceRepository.UpdateAsync(finance, f =>
    // f.Amount); //Assert.Equal(1, reuslt);

    // //单实体更新(不指定列)，有效。 //var reuslt1 = await _userFinanceRepository.UpdateAsync(finance);
    // //Assert.Equal(1, reuslt1);

    // //批量更新不能使用到并发字段,我加了逻辑判断，如果存在rowversion列的实体，不能使用该方法。 var reuslt2 = await
    // _userFinanceRepository.UpdateRangeAsync(u => u.ID == 4, u => new SysUserFinance { Amount
    // = 410 }); Assert.Equal(1, reuslt2);

    //}

    //[Fact]
    //public async Task TestLoading()
    //{
    //    ////select s.*from sysuer id= 1
    //    var user1 = await _userRepository.FetchAsync(u=>new { u.ID,u.Dept},x => x.ID == 1);
    //    Assert.Null(user1.Dept);

    // ////select s.* from sysuer id=1 var user2 = await _userRepository.GetAll().Select(u =>
    // u).Where(x => x.ID == 1).FirstOrDefaultAsync(); Assert.Null(user2.Dept);

    // //left join Include Include for Iqueryable //预加载，开启了延时加载，延时加载不成功，预加载也会报错 var user3 =
    // await _userRepository.GetAll().Include(u => u.Dept).Where(u => u.ID ==
    // 1).FirstOrDefaultAsync(); Assert.NotNull(user3.Dept);

    // //预加载 /* SELECT `t`.`ID`, `t`.`CreateBy`, `t`.`CreateTime`, `t`.`FullName`,
    // `t`.`ModifyBy` , `t`.`ModifyTime`, `t`.`Num`, `t`.`Pid`, `t`.`Pids`, `t`.`SimpleName` ,
    // `t`.`Tips`, `t`.`Version`, `s0`.`ID`, `s0`.`Account`, `s0`.`Avatar` , `s0`.`Birthday`,
    // `s0`.`CreateBy`, `s0`.`CreateTime`, `s0`.`DeptId`, `s0`.`Email` , `s0`.`ModifyBy`,
    // `s0`.`ModifyTime`, `s0`.`Name`, `s0`.`Password`, `s0`.`Phone` , `s0`.`RoleId`,
    // `s0`.`Salt`, `s0`.`Sex`, `s0`.`Status`, `s0`.`Version` FROM ( SELECT `s`.`ID`,
    // `s`.`CreateBy`, `s`.`CreateTime`, `s`.`FullName`, `s`.`ModifyBy` , `s`.`ModifyTime`,
    // `s`.`Num`, `s`.`Pid`, `s`.`Pids`, `s`.`SimpleName` , `s`.`Tips`, `s`.`Version` FROM
    // `SysDept` `s` WHERE `s`.`ID` = 25 LIMIT 2 ) `t` LEFT JOIN `SysUser` `s0` ON `t`.`ID` =
    // `s0`.`DeptId` ORDER BY `t`.`ID`, `s0`.`ID`
    // */ var dept3 = await _deptRepository.GetAll().Include(u => u).Where(u => u.ID >=
    // 25).ToListAsync(); Assert.NotEmpty(dept3);

    // ////指定加载 left join var user4 = await _userRepository.GetAll().Where(u => u.ID ==
    // 1).Select(u => new SysUser { ID = u.ID, Dept = u.Dept }).FirstOrDefaultAsync(); Assert.NotNull(user4.Dept);

    // //Reference //var user5 = await _userRepository.Table.Select(u => u).Where(x => x.ID ==
    // 1).FirstOrDefaultAsync(); //显示加载报错，因为我关闭了跟踪 //Navigation property 'Dept' on entity of
    // type 'SysUser' cannot be loaded because the entity is not being tracked. Navigation
    // properties can only be loaded for tracked entities. //await
    // _userRepository.Entry<SysUser>(user5).Reference(p => p.Dept).LoadAsync(); //Assert.NotNull(user5.Dept);

    // //延时加载报错，因为我关闭了跟踪。如果打开跟踪，可以正常加载。 //An attempt was made to lazy-load navigation property
    // 'Users' on detached entity of type 'SysDept'. Lazy-loading is not supported for detached
    // entities or entities that are loaded with 'AsNoTracking()'. This exception can be
    // suppressed or logged by passing event ID 'CoreEventId.DetachedLazyLoadingWarning' to the
    // 'ConfigureWarnings' method in 'DbContext.OnConfiguring' or 'AddDbContext'. //var dept1 =
    // await _deptRepository.Table.Where(u => u.ID == 25).SingleAsync(); //Assert.NotEmpty(dept1.Users);

    //}

    //[Fact]
    //public void TestNoTracking()
    //{
    //    //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking
    //    var user1 = _userRepository.GetAll().Select(u => u).Where(u => u.ID == 1).FirstOrDefault();
    //    var result1 = _userRepository.Entry(user1).State;
    //    Assert.Equal(EntityState.Detached, result1);

    //    //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll
    //    //var user2 = _userRepository.Table.Select(u => u).Where(u => u.ID == 1).FirstOrDefault();
    //    //var result2 = _userRepository.Entry(user2).State;
    //    //Assert.Equal(EntityState.Unchanged, result2);
    //}

    //[Fact]
    //public async Task TestUOW()
    //{
    //    _unitOfWork.BeginTransaction();

    // string email = "ads@foxmail.com";

    // try { var user2 = new SysUser { ID = 2, Email = email };

    // var user3 = new SysUser { ID = 3, Email = email };

    // //Ef savechange() var result2 = await _userRepository.UpdateAsync(user2, u => u.Email);
    // var result3 = await _userRepository.UpdateAsync(user3, u => u.Email);

    // //Z.EntityFramework.Plus var result4 = await _userRepository.UpdateRangeAsync(x => x.ID
    // >= 4, x => new SysUser { Email = email });

    // //Z.EntityFramework.Plus var result5 = await _userRepository.DeleteRangeAsync(x => x.ID
    // >= 7);

    // //execute raw sql var result6 = await _userRepository.DeleteAsync(new[] { 5, 6 });

    // throw new Exception("TestUOW");

    // //_unitOfWork.CommitTransaction(); } catch (Exception ex) {
    // _unitOfWork.RollbackTransaction(); }

    //    Assert.True(true);
    //}

    //[Fact]
    //public async Task TestFetch()
    //{
    //    //var user1 = await _userRepository.FetchAsync(e => new { e.ID, Dept = new { e.Dept.ID, e.Dept.FullName } }, x => x.ID == 2);

    // //dynamic userDynamic = new ExpandoObject(); var a = new { ID = 2, Dept = new { ID = 2,
    // FullName = "test" } };

    // dynamic userDynamic = a;

    // var user1 = _mapper.Map<SysUser>(userDynamic);

    // Assert.NotNull(user1); }

    //[Fact]
    //public async Task TestSelect()
    //{
    //    //_output.WriteLine("This is output from {0}", "EfCoreRepositoryTests");

    // //var menus1 = await _menuRepository.SelectAsync(100, q => true, q => q.Levels, true, q
    // => q.Num, true); //var menus2 = await _menuRepository.Table.OrderBy(q =>
    // q.Levels).ThenBy(q => q.Num).Take(100).ToListAsync(); //var menus3 = await
    // _menuRepository.GetAsync(q => q.WithPredict(x => true).WithOrderBy(o => o.OrderBy(x =>
    // x.Levels).ThenBy(x => x.Num))); //var menus4 = await
    // _menuRepository.Table.Take(100).OrderBy(q => q.Levels).ThenBy(q => q.Num).ToListAsync();
    // //menus1与menus2,menus1与menus3 sql语句一样，menus3是先取100条再排 //Assert.NotEmpty(menus1);

    // //var user2 = await _userRepository.Table.Select().ToListAsync(); //SELECT 所以列 FROM
    // `SysUser` AS `s` // Assert.NotNull(user2);

    // //var user2 = await _userRepository.Table.Select(e => new SysUser() { Email = e.Email, ID
    // = e.ID }).ToListAsync(); //SELECT `s`.`Email`, `s`.`ID` FROM `SysUser` AS `s` // Assert.NotNull(user2);

    // var user3 = await _userRepository.GetAll().Select(e => new{ e.Name,
    // e.Dept}).ToListAsync(); var user4 = await _userRepository.GetAll().Select(e => new
    // SysUser()).FirstOrDefaultAsync(); //SELECT `s`.`ID`,`s`.`*` FROM `SysUser` AS `s`LEFT
    // JOIN `SysDept` AS `s0` ON `s`.`DeptId` = `s0`.`ID` 左连接了dept表 //Assert.NotNull(user3); //
    // //var user4 = await _deptRepository.Table.SelectMany(d =>
    // d.SysUser).Where(u=>u.Email==null)ToListAsync(); //Assert.NotNull(user4);

    //    //var user5 = await _deptRepository.Table.SelectMany(
    //    //    d => d.SysUser
    //    //    , (t, s) => new { t.ID, t.FullName })
    //    //    .Where(q => q.ID > 4)
    //    //    .ToListAsync();
    //    //SELECT `s`.`ID`, `s`.`FullName`FROM `SysDept` AS `s`INNER JOIN `SysUser` AS `s0` ON `s`.`ID` = `s0`.`DeptId` WHERE `s`.`ID` > 4
    //    //Assert.NotNull(user5);
    //}

    #endregion old testing codes
}
