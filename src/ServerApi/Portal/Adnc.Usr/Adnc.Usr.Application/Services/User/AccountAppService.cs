using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Core.Shared.IRepositories;
using Adnc.Usr.Core.Entities;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Usr.Core.IRepositories;
using Adnc.Core.Shared.Entities;
using Adnc.Application.Shared;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Extensions;
using System.Dynamic;

namespace Adnc.Usr.Application.Services
{
    public class AccountAppService : IAccountAppService
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

        public async Task<UserInfoDto> GetUserInfo(long id)
        {
            var user = await _userRepository.FetchAsync(u => new { u.Account, u.Avatar, u.Birthday, u.DeptId, Dept = new { u.Dept.FullName}, u.Email, u.ID, u.Name, u.Phone, u.RoleId, u.Sex, u.Status }
            , x => x.ID == id);

            UserInfoDto userContext = new UserInfoDto
            {
                Name = user.Name,
            };
            userContext.Profile = _mapper.Map<UserProfileDto>(user);
            userContext.Profile.DeptFullName = user.Dept.FullName;
            if (!string.IsNullOrWhiteSpace(user.RoleId))
            {
                var roleIds = user.RoleId.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
                var roles = await _roleRepository.SelectAsync(r => new { r.ID, r.Tips, r.Name }, x => roleIds.Contains(x.ID));
                foreach (var role in roles)
                {
                    userContext.Roles.Add(role.Tips);
                    userContext.Profile.Roles.Add(role.Name);
                }

                var roleMenus = await _menuRepository.GetMenusByRoleIdsAsync(roleIds.ToArray(), true);
                if (roleMenus.Any())
                {
                    userContext.Permissions.AddRange(roleMenus.Select(x => x.Url).Distinct());
                }
            }

            return userContext;
        }

        public async Task<UserValidateDto> UpdatePassword(UserChangePwdInputDto passwordDto, CurrenUserInfoDto currentUser)
        {
            if (string.Equals(currentUser.Account, "admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"不能修改超级管理员密码"));
            }

            if (!string.Equals(passwordDto.Password, passwordDto.RePassword))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"新密码前后不一致"));
            }

            var user = (await _userRepository.FetchAsync(x => new { x.Password, x.Salt, x.Name, x.Email, x.RoleId, x.Account, x.ID, x.Status }, x => x.ID == currentUser.ID)).To<SysUser>();
            if (!string.Equals(HashHelper.GetHashedString(HashType.MD5, passwordDto.OldPassword, user.Salt), user.Password, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden, "旧密码输入错误"));
            }
            await _userRepository.UpdateAsync(user, p => p.Password);

            return _mapper.Map<UserValidateDto>(user);
        }

        public async Task<UserValidateDto> Login(UserValidateInputDto inputDto, CurrenUserInfoDto currentUser)
        {
            //var user4 = _userRepository.GetAll<SysMenu>().FirstOrDefault();
            //var user0 = _rsp.GetAll<SysUser>().FirstOrDefault();
            var user = await _userRepository.FetchAsync(x => new { x.Password, x.Salt, x.Name, x.Email, x.RoleId,x.Account,x.ID,x.Status }, x => x.Account == inputDto.Account);

            dynamic log = new ExpandoObject();
            log.ID = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            log.Account = inputDto.Account;
            log.CreateTime = DateTime.Now;
            log.Device = currentUser.Device;
            log.RemoteIpAddress = currentUser.RemoteIpAddress;
            log.Message = string.Empty;
            log.Succeed = false;
            log.UserId = user?.ID;
            log.UserName = user?.Name;

            if (user == null)
            {
                var errorModel = new ErrorModel(ErrorCode.NotFound,"用户名或密码错误");
                log.Message = JsonSerializer.Serialize(errorModel);
                throw new BusinessException(errorModel);
            }
            else
            {
                if (user.Status != 1)
                {
                    var errorModel = new ErrorModel(ErrorCode.TooManyRequests, "账号已锁定");
                    log.Message = JsonSerializer.Serialize(errorModel);
                    _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                    throw new BusinessException(errorModel);
                }

                //var logins = await _loginLogRepository.SelectAsync(5, x => new { x.ID, x.Succeed,x.CreateTime }, x => x.UserId == user.ID, x => x.ID, false);
                //var failLoginCount = logins.Count(x => x.Succeed == false);

                var failLoginCount = 2;

                if (failLoginCount == 5)
                {
                    var errorModel = new ErrorModel(ErrorCode.TooManyRequests,"连续登录失败次数超过5次，账号已锁定");
                    log.Message = JsonSerializer.Serialize(errorModel);
                    await _userRepository.UpdateAsync(new SysUser() { ID = user.ID, Status = 2 }, x => x.Status);

                    throw new BusinessException(errorModel);
                }

                if (HashHelper.GetHashedString(HashType.MD5, inputDto.Password, user.Salt) != user.Password)
                {
                    var errorModel = new ErrorModel(ErrorCode.NotFound,"用户名或密码错误");
                    log.Message = JsonSerializer.Serialize(errorModel);
                    _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                    throw new BusinessException(errorModel);
                }

                if (string.IsNullOrEmpty(user.RoleId))
                {
                    var errorModel = new ErrorModel(ErrorCode.Forbidden, "未分配任务角色，请联系管理员");
                    log.Message = JsonSerializer.Serialize(errorModel);
                    _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                    throw new BusinessException(errorModel);
                }
            }

            log.Message = "登录成功";
            log.Succeed = true;
            _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
            return _mapper.Map<UserValidateDto>(user);
        }

        public async Task<UserValidateDto> GetUserValidateInfo(RefreshTokenInputDto tokenInfo)
        {
            var user = await _userRepository.FetchAsync(x => new { x.Name, x.Email, x.RoleId, x.Account, x.ID, x.Status }, x => x.Account == tokenInfo.Account);

            if (user == null)
            {
                throw new BusinessException(new ErrorModel(ErrorCode.NotFound,"用户不存在,参数信息不完整"));
            }

            return _mapper.Map<UserValidateDto>(user);
        }
    }
}
