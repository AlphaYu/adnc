using System;
using System.Net;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Core.Shared.IRepositories;
using Adnc.Usr.Core.Entities;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Usr.Core.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Services;

namespace Adnc.Usr.Application.Services
{
    public class AccountAppService : AppService, IAccountAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysRole> _roleRepository;
        private readonly IEfRepository<SysMenu> _menuRepository;
        private readonly RabbitMqProducer _mqProducer;

        public AccountAppService(IMapper mapper,
            IEfRepository<SysUser> userRepository,
            IEfRepository<SysRole> roleRepository,
            IEfRepository<SysMenu> menuRepository,
            RabbitMqProducer mqProducer)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _mqProducer = mqProducer;
        }

        public async Task<AppSrvResult<UserInfoDto>> GetUserInfoAsync(long id)
        {
            var userProfile = await _userRepository.FetchAsync(u => new UserProfileDto
            {
                Account = u.Account
                ,
                Avatar = u.Avatar
                ,
                Birthday = u.Birthday
                ,
                DeptId = u.DeptId
                ,
                DeptFullName = u.Dept.FullName
                ,
                Email = u.Email
                ,
                Name = u.Name
                ,
                Phone = u.Phone
                ,
                RoleIds = u.RoleIds
                ,
                Sex = u.Sex
                ,
                Status = u.Status
            }
            , x => x.Id == id);

            if (userProfile == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在");

            var userInfoDto = new UserInfoDto { Id = id, Profile = userProfile };

            if (userProfile.RoleIds.IsNotNullOrEmpty())
            {
                var roleIds = userProfile.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
                var roles = await _roleRepository
                                  .Where(x => roleIds.Contains(x.Id))
                                  .Select(r => new { r.Id, r.Tips, r.Name })
                                  .ToListAsync();
                foreach (var role in roles)
                {
                    userInfoDto.Roles.Add(role.Tips);
                    userInfoDto.Profile.Roles.Add(role.Name);
                }

                var roleMenus = await _menuRepository.GetMenusByRoleIdsAsync(roleIds.ToArray(), true);
                if (roleMenus?.Count > 0)
                {
                    userInfoDto.Permissions.AddRange(roleMenus.Select(x => x.Url).Distinct());
                }
            }

            return userInfoDto;
        }

        public async Task<AppSrvResult> UpdatePasswordAsync(long id, UserChangePwdDto input)
        {
            var user = await _userRepository.FetchAsync(x => new { x.Password, x.Salt, x.Id, x.Status }, x => x.Id == id);
            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

            var md5OldPwdString = HashHelper.GetHashedString(HashType.MD5, input.OldPassword, user.Salt);
            if (!md5OldPwdString.EqualsIgnoreCase(user.Password))
                return Problem(HttpStatusCode.BadRequest, "旧密码输入错误");

            var newPwdString = HashHelper.GetHashedString(HashType.MD5, input.Password, user.Salt);

            await _userRepository.UpdateAsync(new SysUser { Id = id, Password = newPwdString }, UpdatingProps<SysUser>(x => x.Password));

            return AppSrvResult();
        }

        public async Task<AppSrvResult<UserValidateDto>> LoginAsync(UserLoginDto inputDto)
        {
            var user = await _userRepository.FetchAsync(x => new
            {
                x.Password
                ,
                x.Salt
                ,
                x.Status
                ,
                UserValidateInfo = new UserValidateDto
                {
                    Account = x.Account
                    ,
                    Email = x.Email
                    ,
                    Id = x.Id
                    ,
                    Name = x.Name
                    ,
                    RoleIds = x.RoleIds
                }
            }
            , x => x.Account == inputDto.Account);

            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户名或密码错误");

            dynamic log = new ExpandoObject();
            log.Account = inputDto.Account;
            log.CreateTime = DateTime.Now;
            var httpContext = HttpContextUtility.GetCurrentHttpContext();
            log.Device = httpContext.Request.Headers["device"].FirstOrDefault() ?? "web";
            log.RemoteIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            log.Succeed = false;
            log.UserId = user.UserValidateInfo.Id;
            log.UserName = user.UserValidateInfo.Name;

            if (user.Status != 1)
            {
                var problem = Problem(HttpStatusCode.TooManyRequests, "账号已锁定");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                return problem;
            }

            //var logins = await _loginLogRepository.SelectAsync(5, x => new { x.Id, x.Succeed,x.CreateTime }, x => x.UserId == user.Id, x => x.Id, false);
            //var failLoginCount = logins.Count(x => x.Succeed == false);

            var failLoginCount = 2;

            if (failLoginCount == 5)
            {
                var problem = Problem(HttpStatusCode.TooManyRequests, "连续登录失败次数超过5次，账号已锁定");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                await _userRepository.UpdateAsync(new SysUser() { Id = user.UserValidateInfo.Id, Status = 2 }, UpdatingProps<SysUser>(x => x.Status));
                _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                return problem;
            }

            if (HashHelper.GetHashedString(HashType.MD5, inputDto.Password, user.Salt) != user.Password)
            {
                var problem = Problem(HttpStatusCode.BadRequest, "用户名或密码错误");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                return problem;
            }

            if (user.UserValidateInfo.RoleIds.IsNullOrEmpty())
            {
                var problem = Problem(HttpStatusCode.Forbidden, "未分配任务角色，请联系管理员");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                return problem;
            }

            log.Message = "登录成功";
            log.StatusCode = (int)HttpStatusCode.Created;
            log.Succeed = true;
            _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);

            return user.UserValidateInfo;
        }

        public async Task<AppSrvResult<UserValidateDto>> GetUserValidateInfoAsync(string account)
        {
            var userValidateInfo = await _userRepository.FetchAsync(x => new UserValidateDto
            {
                Name = x.Name
                ,
                Email = x.Email
                ,
                RoleIds = x.RoleIds
                ,
                Id = x.Id
                ,
                Account = x.Account
            }
            , x => x.Account == account);

            if (userValidateInfo == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

            return userValidateInfo;
        }
    }
}
