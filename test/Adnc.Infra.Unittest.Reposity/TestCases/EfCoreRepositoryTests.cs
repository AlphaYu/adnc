namespace Adnc.Infra.Unittest.Reposity.TestCases;

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

    }

    private static Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;

    /// <summary>
    /// 新增1条件记录
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Never 不会开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded 不会开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Always 会开启1次事物
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestInsertSingle01()
    {
        var customter = await GenerateCustomer();
        var id = customter.Id;
        await _customerRsp.InsertAsync(customter);

        var newCust = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT *  FROM Customer WHERE Id=@Id", new { Id = id });
        Assert.NotEmpty(newCust);
    }

    /// <summary>
    /// 新增2条件记录，不是批量新增
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Never 不会开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded 不会开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Always 会开启2次事物
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestInsertSingle02()
    {
        var customter = await GenerateCustomer();
        var id = customter.Id;
        await _customerRsp.InsertAsync(customter);

        var otherCustomter = await GenerateCustomer();
        var otherId = otherCustomter.Id;
        await _customerRsp.InsertAsync(otherCustomter);

        var customers = await _customerRsp.AdoQuerier.QueryAsync<List<Customer>>("SELECT *  FROM Customer WHERE Id in @Ids", new { ids = new[] { id, otherId } });
        Assert.Equal(2, customers.Count());
    }

    /// <summary>
    /// 新增主从
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Never 不会开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded 会开1次事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Always 会开启1次事物
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestInsert()
    {
        var cusotmer = await GenerateCustomer();
        cusotmer.FinanceInfo = new CustomerFinance { Id = cusotmer.Id, Account = $"{cusotmer.Account}", Balance = 0 };
        var id = cusotmer.Id;

        await _customerRsp.InsertAsync(cusotmer);

        var newCust = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT *  FROM Customer WHERE Id=@Id", new { Id = id });
        Assert.NotEmpty(newCust);
    }

    /// <summary>
    /// 批量新增，开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Never 不会开启事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded 会开1次事物
    /// Database.AutoTransactionBehavior = AutoTransactionBehavior.Always 会开启1次事物
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestInsertRange()
    {
        var customer = await _customerRsp.FetchAsync(x => x, x => x.Id > 1);

        var id0 = UnittestHelper.GetNextId();
        var id1 = UnittestHelper.GetNextId();
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
        var result2 = await _customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID in @ids", new { ids = customers.Select(x => x.Id) });
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
        //var cus1 = await InsertCustomer();
        //var cus2 = await _customerRsp.FindAsync(x => x.Id < cus1.Id, navigationPropertyPath: null, x => x.Id, false, noTracking: false);
        //cus2.Nickname = "string";
        //var cus3 = await _customerRsp.FindAsync(x => x.Id < cus2.Id);
        //var cus4 = (await _customerRsp.AdoQuerier.QueryAsync<Customer>($"SELECT * FROM Customer  WHERE ID<{cus3.Id} ORDER BY ID ASC  LIMIT 0,1")).FirstOrDefault();

        //var propertyNameAndValues = new Dictionary<long, List<(string propertyName, dynamic propertyValue)>>
        //{
        //    {
        //        cus1.Id,
        //        new List<(string Column, dynamic Value)>
        //        {
        //           ("Realname",UnittestHelper.GetNextId().ToString()),
        //           ("Nickname","Nickname1111")
        //        }
        //    },
        //    {
        //        cus2.Id,
        //        new List<(string Column, dynamic Value)>
        //        {
        //           ("Realname",UnittestHelper.GetNextId().ToString()),
        //           ("Nickname","Nickname2222")
        //        }
        //    },
        //    {
        //        cus3.Id,
        //        new List<(string Column, dynamic Value)>
        //        {
        //           ("Realname",UnittestHelper.GetNextId().ToString()),
        //           ("Nickname","Nickname3333")
        //        }
        //    }
        //    ,
        //    {
        //        cus4.Id,
        //        new List<(string Column, dynamic Value)>
        //        {
        //           ("Realname",UnittestHelper.GetNextId().ToString()),
        //           ("Nickname","Nickname4444")
        //        }
        //    }
        //};

        //var total = await _customerRsp.UpdateRangeAsync(propertyNameAndValues);

        //_output.WriteLine(total.ToString());

        //Assert.Equal(4, total);
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
        var total = await _customerRsp.DeleteAsync(99872221111111111);
        Assert.Equal(0, total);
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
        var customerQueryable = _customerRsp.GetAll().Where(x => x.Id > 10);
        var custsLogsQueryable = _custLogsRsp.GetAll().Where(x => x.Id > 88);

        //SELECT `t`.`id` AS `Id`, `t`.`customerid` AS `CustomerId`, `t`.`account` AS `Account`, `t`.`changedamount` AS `ChangedAmount`, `t`.`changingamount` AS `ChangingAmount`, `c`.`realname` AS `Realname`
        //FROM `customer` AS `c`
        //INNER JOIN(
        //    SELECT `c0`.`id`, `c0`.`account`, `c0`.`changedamount`, `c0`.`changingamount`, `c0`.`customerid`
        //    FROM `customertransactionlog` AS `c0`
        //    WHERE `c0`.`id` > 88
        //) AS `t` ON `c`.`id` = `t`.`customerid`
        //WHERE(`c`.`id` > 10) AND(`t`.`changedamount` = 0.0)
        var logs = await customerQueryable.Join(custsLogsQueryable, c => c.Id, t => t.CustomerId, (c, t) => new
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
        .Where(d => d.ChangedAmount == 0)
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
}
