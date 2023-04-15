using Adnc.Infra.IdGenerater.Yitter;
using Adnc.Infra.IRepositories;
using Adnc.Infra.Mapper;
using Adnc.UnitTest.TestCases.Repositories.Dtos;
using Adnc.UnitTest.TestCases.Repositories.Entities;

namespace Adnc.UnitTest.TestCases.Repositories
{
    public class MapperTests : IClassFixture<EfCoreDbcontextFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly IEfRepository<Customer> _customerRsp;
        private readonly IObjectMapper _objectMapper;
        private readonly DbContext _dbContext;
        private readonly EfCoreDbcontextFixture _fixture;

        public MapperTests(EfCoreDbcontextFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _customerRsp = _fixture.Container.GetRequiredService<IEfRepository<Customer>>();
            _dbContext = _fixture.Container.GetRequiredService<DbContext>();
            _objectMapper = _fixture.Container.GetRequiredService<IObjectMapper>();
            if (IdGenerater.CurrentWorkerId < 0)
                IdGenerater.SetWorkerId(1);
        }

        [Fact]
        public async Task Test_Dto_MapperTo_Entity_WithTraking_01_True()
        {
            var customerDto = new CustomerDto
            {
                Nickname = Guid.NewGuid().ToString().Substring(0, 10) ,
                Realname = Guid.NewGuid().ToString().Substring(0, 10) ,
                Password = ""
            };

            try
            {
                var customer = await _customerRsp.FindAsync(x => x.Id > 1, noTracking: false);
                var newCustmer = _objectMapper.Map(customerDto, customer);
                await _customerRsp.UpdateAsync(newCustmer);

                var result = await _customerRsp.AdoQuerier.QuerySingleAsync<CustomerDto>($"SELECT * FROM Customer where id={newCustmer.Id}");

                Assert.True(result.Realname == customerDto.Realname);
                Assert.True(result.Nickname == customerDto.Nickname);
                Assert.True(result.Password == customerDto.Password);

                var account = await _customerRsp.AdoQuerier.QuerySingleAsync<string>($"SELECT account FROM Customer where id={newCustmer.Id}");
                Assert.True(!string.IsNullOrWhiteSpace(account));

            }
            catch (Exception ex)
            {
                _output.WriteLine(ex.ToString());
                Assert.False(false);
            }
        }

        [Fact]
        public async Task Test_Dto_MapperTo_Entity_WithTraking_02_True()
        {
            var customerDto = new CustomerDto
            {
                Nickname = Guid.NewGuid().ToString().Substring(0, 10),
                Realname = Guid.NewGuid().ToString().Substring(0, 10),
                Password = ""
            };

            try
            {
                var customer = await _customerRsp.FindAsync(x => x.Id > 1, noTracking: false);
                var newCustmer = _objectMapper.Map<Customer>(customerDto);
                await _customerRsp.UpdateAsync(newCustmer);

                var result = await _customerRsp.AdoQuerier.QuerySingleAsync<CustomerDto>($"SELECT * FROM Customer where id={newCustmer.Id}");

                Assert.True(result.Realname == customerDto.Realname);
                Assert.True(result.Nickname == customerDto.Nickname);
                Assert.True(result.Password == customerDto.Password);

                var account = await _customerRsp.AdoQuerier.QuerySingleAsync<string>($"SELECT account FROM Customer where id={newCustmer.Id}");
                Assert.True(!string.IsNullOrWhiteSpace(account));

            }
            catch (Exception ex)
            {
                _output.WriteLine(ex.ToString());
                Assert.False(false);
            }
        }
    }
}
