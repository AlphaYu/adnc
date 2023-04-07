using System;
using System.Threading.Tasks;
using Xunit;
using Autofac;
using Adnc.Application.Services;
using Adnc.Application.Dtos;
using Adnc.Common.Helper;

namespace Adnc.UnitTest
{
    public class ApplicationServicesTests : IClassFixture<EfCoreDbcontextFixture>
    {
        //private readonly ITestOutputHelper _output;
        private readonly ILogAppService _logAppService;
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;

        private EfCoreDbcontextFixture _fixture;

        public ApplicationServicesTests(EfCoreDbcontextFixture fixture)
        {
            _fixture = fixture;
            //_output = output;
            _logAppService = _fixture.Container.Resolve<ILogAppService>();
            _userAppService = _fixture.Container.Resolve<IUserAppService>();
            _roleAppService = _fixture.Container.Resolve<IRoleAppService>();
        }

        [Fact]
        public async Task TesRoleAppService()
        {
            var result4 = await _roleAppService.GetRoleTreeListByUserId(17);
            Assert.NotNull(result4);

            var result3 = await _roleAppService.GetPaged(new RoleSearchDto { PageIndex = 1, PageSize = 100 });
            Assert.NotNull(result3);

            var result2 = await _roleAppService.Delete(4);
            Assert.Equal(1, result2);

            var rolePremisson = new PermissonSaveInputDto
            {
                RoleId = 4
                //Permissions = "1,21,28,2,41,68,69,42,70,43,66,67,44,45,65,46"
            };

            var result1 = await _roleAppService.SaveRolePermisson(rolePremisson);
            Assert.Equal(1, result1);

            var role = new RoleSaveInputDto()
            {
                Pid = 1,
                DeptId = 24,
                Name = $"测试角色{SecurityHelper.GenerateRandomCode(4)}",
                Tips = $"testing role {SecurityHelper.GenerateRandomCode(4)}",
                Version =1,
                Num =3
            };

            var result = await _roleAppService.Save(role);
            Assert.Equal(1, result);
        }


        [Fact]
        public async Task TesUserAppService()
        {
            var user6 = new UserSaveInputDto()
            {
                ID =17,
                Account = SecurityHelper.GenerateRandomCode(6),
                Avatar = "",
                Birthday = DateTime.Now.AddYears(-20),
                DeptId = 25,
                Email = "alpha@google.com",
                Name = "王大户",
                Password = "123321",
                Phone = "",
                Sex = 1
            };
            var result6 = await _userAppService.Save(user6);
            Assert.Equal(1, result6);

            var reuslt5 = await _userAppService.GetPaged(new UserSearchDto() { PageIndex = 0, PageSize = 100 });
            Assert.NotEmpty(reuslt5.Data);

            var result4 = await _userAppService.SetRole(new RoleSetInputDto { ID = 17, RoleIds = "2,3," });
            Assert.Equal(1, result4);

            var result3 = await _userAppService.ChangeStatus(17);
            Assert.Equal(1, result3);

            var result2 = await _userAppService.Delete(17);
            Assert.Equal(1, result2);

            var user1 = new UserSaveInputDto()
            {
                Account = SecurityHelper.GenerateRandomCode(6),
                Avatar = "",
                Birthday = DateTime.Now.AddYears(-20),
                DeptId = 25,
                Email = "alpha@google.com",
                Name = "王大户",
                Password = "123321",
                Phone = "",
                Sex = 1
            };
            var result1 = await _userAppService.Save(user1);
            Assert.Equal(1,result1);
        }


        [Fact]
        public async Task TestLogAppService()
        {
            var log = new OpsLogSaveInputDto()
            {
                ClassName = "111",
                UserId = 4,
                Message = "test"
            };

           //var result2 = await _logAppService.(log);
           // Assert.Equal(1,result2);


            var searchModel = new LogSearchDto
            {
                BeginTime = DateTime.Now.AddDays(-720),
                EndTime = DateTime.Now,
                PageIndex=1,
                PageSize =10
            };
            var result1 = await _logAppService.GetOpsLogsPaged(searchModel);
            Assert.NotNull(result1);
        }
    }
}
