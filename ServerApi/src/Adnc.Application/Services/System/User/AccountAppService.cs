using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Common;
using Adnc.Application.Dtos;
using Adnc.Common.Models;
using Adnc.Common.Extensions;
using Adnc.Common.Helper;
using Adnc.Core.IRepositories;
using Adnc.Core.Entities;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Adnc.Application.Services
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IMapper _mapper;
        private readonly UserContext _currentUser;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysRole> _roleRepository;
        private readonly IEfRepository<SysLoginLog> _loginLogRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly UserContext _userContext;

        public AccountAppService(IMapper mapper,
            UserContext currentUser,
            IEfRepository<SysUser> userRepository,
            IEfRepository<SysRole> roleRepository,
            IEfRepository<SysLoginLog> loginLogRepository,
            IMenuRepository menuRepository,
            UserContext userContext)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _loginLogRepository = loginLogRepository;
            _userContext = userContext;
        }

        public async Task<UserInfoDto> GetCurrentUserInfo()
        {
            var user = await _userRepository.FetchAsync(u => new { u.ID, u.RoleId, u.Name, Dept = new { u.Dept.FullName } }, x => x.ID == _currentUser.ID);

            UserInfoDto userContext = new UserInfoDto
            {
                Name = user.Name,
                Role = "admin"
            };
            userContext.Profile = _mapper.Map<UserProfileDto>(user);
            userContext.Profile.Dept = user.Dept.FullName;
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

        public async Task UpdatePassword(UserChangePwdInputDto passwordDto)
        {
            if (string.Equals(_currentUser.Account, "admin", StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"不能修改超级管理员密码"));
            }

            if (!string.Equals(passwordDto.Password, passwordDto.RePassword))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"新密码前后不一致"));
            }

            var user = await _userRepository.FetchAsync(u => new { u.ID, u.Password,u.Salt }, x => x.ID == _currentUser.ID);
            if (!string.Equals(HashHelper.GetHashedString(HashType.MD5, passwordDto.OldPassword, user.Salt), user.Password, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"旧密码输入错误"));
            }
            await _userRepository.UpdateAsync(user, p => p.Password);
        }

        public async Task<UserValidateDto> Login(UserValidateInputDto inputDto)
        {
            var user = await _userRepository.FetchAsync(x => new { x.Password, x.Salt, x.Name, x.Email, x.RoleId,x.Account,x.ID,x.Status }, x => x.Account == inputDto.Account);

            var log = new SysLoginLog()
            {
                ID = new Snowflake(1,1).NextId(),
                Account = inputDto.Account,
                CreateTime = DateTime.Now,
                Device = "web",
                RemoteIpAddress = _userContext.RemoteIpAddress,
                Message = string.Empty,
                Succeed = false,
                UserId = user?.ID,
                UserName = user?.Name,
            };

            if (user == null)
            {
                var errorModel = new ErrorModel(ErrorCode.NotFound,"用户名或密码错误");
                log.Message = JsonConvert.SerializeObject(errorModel);

                throw new BusinessException(errorModel);
            }
            else
            {
                if (user.Status != 1)
                {
                    var errorModel = new ErrorModel(ErrorCode.TooManyRequests, "账号已锁定");
                    log.Message = JsonConvert.SerializeObject(errorModel);
                    await _loginLogRepository.InsertAsync(log);

                    throw new BusinessException(errorModel);
                }

                var logins = await _loginLogRepository.SelectAsync(5, x => new { x.ID, x.Succeed,x.CreateTime }, x => x.UserId == user.ID, x => x.ID, false);
                var failLoginCount = logins.Count(x => x.Succeed == false);

                if (failLoginCount == 5)
                {
                    var errorModel = new ErrorModel(ErrorCode.TooManyRequests,"连续登录失败次数超过5次，账号已锁定");
                    log.Message = JsonConvert.SerializeObject(errorModel);
                    await _userRepository.UpdateAsync(new SysUser() { ID = user.ID, Status = 2 }, x => x.Status);

                    throw new BusinessException(errorModel);
                }

                if (HashHelper.GetHashedString(HashType.MD5, inputDto.Password, user.Salt) != user.Password)
                {
                    var errorModel = new ErrorModel(ErrorCode.NotFound,"用户名或密码错误");
                    log.Message = JsonConvert.SerializeObject(errorModel);
                    await _loginLogRepository.InsertAsync(log);

                    throw new BusinessException(errorModel);
                }

                if (string.IsNullOrEmpty(user.RoleId))
                {
                    var errorModel = new ErrorModel(ErrorCode.Forbidden, "未分配任务角色，请联系管理员");
                    log.Message = JsonConvert.SerializeObject(errorModel);
                    await _loginLogRepository.InsertAsync(log);

                    throw new BusinessException(errorModel);
                }
            }

            log.Message = "登录成功";
            log.Succeed = true;
            await _loginLogRepository.InsertAsync(log);

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
