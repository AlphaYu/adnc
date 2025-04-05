using Microsoft.EntityFrameworkCore.ChangeTracking;
using MongoDB.Driver.Core.Misc;

namespace Adnc.Infra.Unittest.Reposity.TestCases;

public class EfCoreRepositoryTests(EfCoreDbcontextFixture fixture, ITestOutputHelper output) : IClassFixture<EfCoreDbcontextFixture>
{
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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        var customter = GenerateCustomer();
        var id = customter.Id;
        await customerRsp.InsertAsync(customter);

        var newCust = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT *  FROM customer WHERE Id=@Id", new { Id = id });
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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var customter = GenerateCustomer();
        var id = customter.Id;
        await customerRsp.InsertAsync(customter);

        var otherCustomter = GenerateCustomer();
        var otherId = otherCustomter.Id;
        await customerRsp.InsertAsync(otherCustomter);

        var customers = await customerRsp.AdoQuerier.QueryAsync<List<Customer>>("SELECT *  FROM customer WHERE Id in @Ids", new { ids = new[] { id, otherId } });
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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var cusotmer = GenerateCustomer();
        cusotmer.FinanceInfo = new CustomerFinance { Id = cusotmer.Id, Account = $"{cusotmer.Account}", Balance = 0 };
        var id = cusotmer.Id;

        await customerRsp.InsertAsync(cusotmer);

        var newCust = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT *  FROM customer WHERE Id=@Id", new { Id = id });
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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        var custLogsRsp = fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();

        var customer = await customerRsp.FetchAsync(x => x.Id > 1);

        var id0 = UnittestHelper.GetNextId();
        var id1 = UnittestHelper.GetNextId();
        var logs = new List<CustomerTransactionLog>
        {
            new CustomerTransactionLog{ Id=id0,Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeBehavior.Recharge,ExchageStatus=ExchageStatus.Finished,Remark="test"}
            ,
            new CustomerTransactionLog{ Id=id1,Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeBehavior.Recharge,ExchageStatus=ExchageStatus.Finished,Remark="test"}
        };

        await custLogsRsp.InsertRangeAsync(logs);

        var logsFromDb = await custLogsRsp.Where(x => x.Id == id0 || x.Id == id1).ToListAsync();
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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();

        var ids = await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM customer where ID in @ids ORDER BY ID", new { ids = new[] { cus1.Id, cus2.Id } });
        var id0 = ids.ToArray()[0];

        //IEfRepository<>默认关闭了跟踪，需要手动开启跟踪
        var customer = await customerRsp.FindAsync(id0, noTracking: false);
        //实体已经被跟踪
        customer.Realname = "被跟踪01";
        await customerRsp.UpdateAsync(customer);
        var newCust1 = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE Id=@Id", new { Id = id0 });
        Assert.Equal("被跟踪01", newCust1.FirstOrDefault().Realname);

        var customerId = (await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id  FROM customerfinance limit 0,1")).FirstOrDefault();
        customer = await customerRsp.FindAsync(x => x.Id == customerId, x => x.FinanceInfo, noTracking: false);
        customer.Account = "主从更新01";
        customer.FinanceInfo.Account = "主从更新01";
        await customerRsp.UpdateAsync(customer);
        var newCust2 = await customerRsp.FindAsync(customerId, x => x.FinanceInfo);
        Assert.Equal("主从更新01", newCust2.Account);
        Assert.Equal("主从更新01", newCust2.FinanceInfo.Account);
    }

    /// <summary>
    /// 更新，指定列
    /// </summary>
    [Fact]
    public async Task TestUpdateAssigns()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();
        var cus3 = await InsertCustomer();
        var cus4 = await InsertCustomer();

        var ids = await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM customer where ID in @ids ORDER BY ID", new { ids = new[] { cus1.Id, cus2.Id, cus3.Id, cus4.Id } });
        var id0 = ids.ToArray()[0];
        var customer = await customerRsp.FetchAsync(x=>x.Id == id0, noTracking: false);
        //实体已经被跟踪并且指定更新列,     更新列没有指定Realname，该列不会被更新
        customer.Nickname = "更新指定列";
        customer.Realname = "不指定该列";
        await customerRsp.UpdateAsync(customer, UpdatingProps<Customer>(c => c.Nickname));
        var newCus = (await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID=@ID", customer)).FirstOrDefault();
        Assert.Equal("更新指定列", newCus.Nickname);
        Assert.NotEqual("不指定该列", newCus.Realname);
    }

    /// <summary>
    /// 批量更新，指定列
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestUpdateRange()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();
        var total = await customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
        Assert.Equal(2, total);

        await customerRsp.UpdateRangeAsync(c => c.Id == cus1.Id || c.Id == cus2.Id, x => new Customer { Realname = "批量更新" });
        var result2 = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var customers = await customerRsp.GetAll(noTracking: false).Skip(0).Take(2).ToListAsync();
        foreach (var customer in customers)
        {
            customer.Realname = "RangeWithTrack";
        }
        await customerRsp.UpdateRangeAsync(customers);
        var result2 = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID in @ids", new { ids = customers.Select(x => x.Id) });
        Assert.NotEmpty(result2);
        Assert.Equal("RangeWithTrack", result2.FirstOrDefault().Realname);
        Assert.Equal("RangeWithTrack", result2.LastOrDefault().Realname);

        var customers2 = await customerRsp.GetAll(noTracking: false).Skip(0).Take(2).ToListAsync();
        foreach (var customer in customers2)
        {
            customer.Realname = "RangeWithNoTrack";
        }
        try
        {
            int rows = await customerRsp.UpdateRangeAsync(customers2);
            Assert.False(rows > 0);
        }
        catch
        {
            output.WriteLine("不允许更新");
        }
    }

    [Fact]
    public async Task TestUpdateRangeDifferentMembers()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();
        cus2.Nickname = "string";
        var cus3 = await InsertCustomer(); 
        var cus4 = await InsertCustomer();

        var propertyNameAndValues = new Dictionary<long, List<(string propertyName, dynamic propertyValue)>>
        {
            {
                cus1.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",UnittestHelper.GetNextId().ToString()),
                   ("Nickname","Nickname1111")
                }
            },
            {
                cus2.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",UnittestHelper.GetNextId().ToString()),
                   ("Nickname","Nickname2222")
                }
            },
            {
                cus3.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",UnittestHelper.GetNextId().ToString()),
                   ("Nickname","Nickname3333")
                }
            }
            ,
            {
                cus4.Id,
                new List<(string Column, dynamic Value)>
                {
                   ("Realname",UnittestHelper.GetNextId().ToString()),
                   ("Nickname","Nickname4444")
                }
            }
        };

        var total = await customerRsp.UpdateRangeAsync(propertyNameAndValues);

        output.WriteLine(total.ToString());

        Assert.Equal(4, total);
    }

    /// <summary>3
    /// 测试删除
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestDelete()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        //删除，无跟踪状态
        var customer = await InsertCustomer();
        var customerFromDb = await customerRsp.FindAsync(customer.Id);
        Assert.Equal(customer.Id, customerFromDb.Id);

        await customerRsp.DeleteAsync(customer.Id);
        var result = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID=@Id", new { customer.Id });
        Assert.Equal(0, result.Count());

        //删除，有跟踪状态
        //var customer2 = await InsertCustomer();
        //await customerRsp.DeleteAsync(customer2.Id);
        //result = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID=@Id", new { customer2.Id });
        //Assert.Equal(0,result.Count());

        //var ids = await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM customer ORDER BY ID DESC LIMIT 1");
        ////删除，上下文中无该实体。
        //var id = ids.First();
        //await customerRsp.DeleteAsync(id);
        //result = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID=@Id", new { Id = id });
        //Assert.Equal(0, result.Count());

        //删除不存在的记录
        var total = await customerRsp.DeleteAsync(99872221111111111);
        Assert.Equal(0, total);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestDeleteRange()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        //batch hand delete
        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();
        var total = await customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
        Assert.Equal(2, total);

        await customerRsp.ExecuteDeleteAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
        var result2 = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
        Assert.Equal(0, result2.Count());
    }

    /// <summary>
    /// 测试条件查询单条记录
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestFetch()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var cus1 = await InsertCustomer();
        var cus2 = await InsertCustomer();

        //指定列查询
        var customer = await customerRsp.FetchAsync(x => new { x.Id, x.Account }, x => x.Id == cus1.Id);
        Assert.NotNull(customer);

        //指定列查询，指定列包含导航属性
        var customer2 = await customerRsp.FetchAsync(x => new { x.Id, x.Account, x.FinanceInfo }, x => x.Id == cus2.Id);
        Assert.NotNull(customer2.FinanceInfo);
    }

    /// <summary>
    /// 测试where and getall
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestWhereAndGetAll()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();
        var custLogsRsp = fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();

        var customers = await customerRsp.Where(x => x.Id > 1).ToListAsync();
        Assert.NotEmpty(customers);

        var customer = await customerRsp.Where(x => x.Id > 1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        Assert.NotNull(customer);

        //GetAll() = Where(x=>true)
        var customerQueryable = customerRsp.GetAll().Where(x => x.Id > 10);
        var custsLogsQueryable = custLogsRsp.GetAll().Where(x => x.Id > 88);

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
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var sql = $@"SELECT `c0`.`Id`, `c0`.`CustomerId`, `c0`.`Account`, `c0`.`ChangedAmount`, `c0`.`ChangingAmount`, `c`.`Realname`
                        FROM `customer` AS `c`
                        INNER JOIN `customertransactionlog` AS `c0` ON `c`.`Id` = `c0`.`CustomerId`
                        WHERE `c0`.`Id` > @Id";
        var logs = await customerRsp.AdoQuerier.QueryAsync(sql, new { Id = 1 });

        Assert.NotEmpty(logs);
    }

    /// <summary>
    /// 测试多种查询
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestReutrnNullResult()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var result = await customerRsp.Where(x => x.Id == 999).FirstOrDefaultAsync();
        Assert.Null(result);

        var dapperResult = await customerRsp.AdoQuerier.QueryFirstOrDefaultAsync<Customer>("select * FROM customer where id=999");
        Assert.Null(dapperResult);

        dynamic dapperResult2 = await customerRsp.AdoQuerier.QueryFirstOrDefaultAsync("select * FROM customer where id=999");
        Assert.Null(dapperResult2);
    }

    /// <summary>
    ///  测试EFCore8 ExcuteDelete是否支持软删除
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task TestEfcoreExcuteDeleteForSoftDelete()
    {
        var custProject = fixture.Container.GetRequiredService<IEfRepository<Project>>();

        //var aa = await custProject.GetAll().SingleOrDefaultAsync();
        //var bb = await custProject.GetAll().FirstOrDefaultAsync(x => x.Id == 999);
        var project = new Project { Id = UnittestHelper.GetNextId(), Name = $"{UnittestHelper.GetNextId()}" };
        await custProject.InsertAsync(project);

        var newProject = await custProject.FetchAsync(x => x.Id == project.Id);

        //DELETE `p`
        //FROM `project` AS `p`
        //WHERE NOT(`p`.`isdeleted`) AND(`p`.`id` = 1740035750473)
        //EFCore8 ExcuteDelete不支持软删除
        await custProject.ExecuteDeleteAsync(x => x.Id == project.Id);
        var result = await custProject.AdoQuerier.QueryFirstOrDefaultAsync<Project>("SELECT * FROM project WHERE ID=@Id", new { Id = project.Id });

        Assert.NotNull(result);
        Assert.True(result.IsDeleted);
    }

    [Fact]
    public async Task TestEfcore8UpdateRangeAsync()
    {
        var custProject = fixture.Container.GetRequiredService<IEfRepository<Project>>();

        var project = new Project { Id = UnittestHelper.GetNextId(), Name = $"{UnittestHelper.GetNextId()}" };
        await custProject.InsertAsync(project);

        var result = await custProject.ExecuteUpdateAsync(x => x.Id == project.Id, setter 
            => setter.SetProperty(x => x.Name, "TestEfcore8UpdateRangeAsync"));
        Assert.True(result > 0);

        var newProject = await custProject.FindAsync(project.Id);
        Assert.Equal(1000000000001, newProject.ModifyBy);
        Assert.Equal("TestEfcore8UpdateRangeAsync", newProject.Name);
    }

    [Fact]
    public async Task TestUpdateRangeAsync()
    {
        var custProject = fixture.Container.GetRequiredService<IEfRepository<Project>>();

        var project = new Project { Id = UnittestHelper.GetNextId(), Name = $"{UnittestHelper.GetNextId()}" };
        await custProject.InsertAsync(project);

        var result = await custProject.UpdateRangeAsync(x => x.Id == project.Id, x => new Project { Name = "abcdef" });
        Assert.True(result > 0);

        var newProject = await custProject.FindAsync(project.Id);
        Assert.Equal(1000000000001, newProject.ModifyBy);
        Assert.Equal("abcdef", newProject.Name);
    }

    private async Task<Customer> InsertCustomer()
    {
        var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

        var id = UnittestHelper.GetNextId();
        var customer = new Customer() { Id = id, Password = "password", Account = "alpha2008", Nickname = UnittestHelper.GetNextId().ToString(), Realname = UnittestHelper.GetNextId().ToString() };
        customer.FinanceInfo = new CustomerFinance { Id = customer.Id, Account = customer.Account, Balance = 0 };
        await customerRsp.InsertAsync(customer);
        return customer;
    }

    private Customer GenerateCustomer()
    {
        var id = UnittestHelper.GetNextId();
        var customer = new Customer() { Id = id, Password = "password", Account = "alpha2008", Nickname = UnittestHelper.GetNextId().ToString(), Realname = UnittestHelper.GetNextId().ToString() };
        return customer;
    }
}
