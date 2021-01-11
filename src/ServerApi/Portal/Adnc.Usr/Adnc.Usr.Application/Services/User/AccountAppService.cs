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
using Adnc.Core.Shared.Entities;
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
        private readonly IEfRepository<NullEntity> _rsp;
        private readonly RabbitMqProducer _mqProducer;

        public AccountAppService(IMapper mapper,
            IEfRepository<SysUser> userRepository,
            IEfRepository<SysRole> roleRepository,
            IEfRepository<SysMenu> menuRepository,
            IEfRepository<NullEntity> rsp,
            RabbitMqProducer mqProducer)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _rsp = rsp;
            _mqProducer = mqProducer;
        }

        public async Task<AppSrvResult<UserInfoDto>> GetUserInfo(long id)
        {
            var user = await _userRepository.FetchAsync(u => new { u.Account, u.Avatar, u.Birthday, u.DeptId, Dept = new { u.Dept.FullName }, u.Email, u.ID, u.Name, u.Phone, u.RoleId, u.Sex, u.Status }
            , x => x.ID == id);

            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在");

            var userInfoDto = new UserInfoDto { Id = user.ID };

            userInfoDto.Profile = _mapper.Map<UserProfileDto>(user);

            if (user.RoleId.IsNotNullOrEmpty())
            {
                var roleIds = user.RoleId.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
                var roles = await _roleRepository.SelectAsync(r => new { r.ID, r.Tips, r.Name }, x => roleIds.Contains(x.ID));
                foreach (var role in roles)
                {
                    userInfoDto.Roles.Add(role.Tips);
                    userInfoDto.Profile.Roles.Add(role.Name);
                }

                var roleMenus = await _menuRepository.GetMenusByRoleIdsAsync(roleIds.ToArray(), true);
                if (roleMenus.Any())
                {
                    userInfoDto.Permissions.AddRange(roleMenus.Select(x => x.Url).Distinct());
                }
            }

            return userInfoDto;
        }

        public async Task<AppSrvResult> UpdatePassword(UserChangePwdInputDto passwordDto,long userId)
        {
            var user = await _userRepository.FetchAsync(x => new { x.Password, x.Salt, x.Name, x.Email, x.RoleId, x.Account, x.ID, x.Status }, x => x.ID == userId);
            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

            var md5OldPwdString = HashHelper.GetHashedString(HashType.MD5, passwordDto.OldPassword, user.Salt);
            if (!md5OldPwdString.EqualsIgnoreCase(user.Password))
                return Problem(HttpStatusCode.BadRequest, "旧密码输入错误");

            var newPwdString = HashHelper.GetHashedString(HashType.MD5, passwordDto.Password, user.Salt);

            await _userRepository.UpdateAsync(new SysUser { ID = userId, Password = newPwdString }, p => p.Password);

            return DefaultResult();
        }

        public async Task<AppSrvResult<UserValidateDto>> Login(UserValidateInputDto inputDto)
        {
            var user = await _userRepository.FetchAsync(x => new { x.Password, x.Salt, x.Name, x.Email, x.RoleId,x.Account,x.ID,x.Status }, x => x.Account == inputDto.Account);

            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户名或密码错误");

            dynamic log = new ExpandoObject();
            log.Account = inputDto.Account;
            log.CreateTime = DateTime.Now;
            var httpContext = HttpContextUtility.GetCurrentHttpContext();
            log.Device = httpContext.Request.Headers["device"].FirstOrDefault() ?? "web";
            log.RemoteIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            log.Succeed = false;
            log.UserId = user?.ID;
            log.UserName = user?.Name;

            if (user.Status != 1)
            {
                var problem = Problem(HttpStatusCode.TooManyRequests, "账号已锁定");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                return problem;
            }

            //var logins = await _loginLogRepository.SelectAsync(5, x => new { x.ID, x.Succeed,x.CreateTime }, x => x.UserId == user.ID, x => x.ID, false);
            //var failLoginCount = logins.Count(x => x.Succeed == false);

            var failLoginCount = 2;

            if (failLoginCount == 5)
            {
                var problem = Problem(HttpStatusCode.TooManyRequests, "连续登录失败次数超过5次，账号已锁定");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                await _userRepository.UpdateAsync(new SysUser() { ID = user.ID, Status = 2 }, x => x.Status);
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

            if (user.RoleId.IsNullOrEmpty())
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

            return _mapper.Map<UserValidateDto>(user);
        }

        public async Task<AppSrvResult<UserValidateDto>> GetUserValidateInfo(RefreshTokenInputDto tokenInfo)
        {
            var user = await _userRepository.FetchAsync(x => new { x.Name, x.Email, x.RoleId, x.Account, x.ID, x.Status }, x => x.Account == tokenInfo.Account);

            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

            return _mapper.Map<UserValidateDto>(user);
        }
    }
}
