﻿using System;
using System.Net;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Core.Shared.IRepositories;
using Adnc.Usr.Core.Entities;
using Adnc.Infra.EventBus.RabbitMq;
using Adnc.Usr.Core.RepositoryExtensions;
using Adnc.Infra.Common.Helper;
using Adnc.Infra.Common.Extensions;
using Adnc.Application.Shared.Services;
using Adnc.Usr.Application.Contracts.Services;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Usr.Application.Caching;

namespace Adnc.Usr.Application.Services
{
    public class AccountAppService : AbstractAppService, IAccountAppService
    {
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysRole> _roleRepository;
        private readonly IEfRepository<SysMenu> _menuRepository;
        private readonly RabbitMqProducer _mqProducer;
        private readonly CacheService _cacheService;

        public AccountAppService(IEfRepository<SysUser> userRepository
           , IEfRepository<SysRole> roleRepository
           , IEfRepository<SysMenu> menuRepository
           , RabbitMqProducer mqProducer
           , CacheService cacheService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _mqProducer = mqProducer;
            _cacheService = cacheService;
        }

        public async Task<AppSrvResult<UserValidateDto>> LoginAsync(UserLoginDto inputDto)
        {
            var exists = await _cacheService.BloomFilters.Accounts.ExistsAsync(inputDto.Account.ToLower());
            if(!exists)
                return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

            var user = await _userRepository.FetchAsync(x => new UserValidateDto()
            {
                Id = x.Id
                ,
                Account = x.Account
                ,
                Password = x.Password
                 ,
                Salt = x.Salt
                 ,
                Status = x.Status
                 ,
                Email = x.Email
                 ,
                Name = x.Name
                    ,
                RoleIds = x.RoleIds
            }, x => x.Account == inputDto.Account);
            if (user == null)
                return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

            dynamic log = new ExpandoObject();
            log.Account = inputDto.Account;
            log.CreateTime = DateTime.Now;
            var httpContext = HttpContextUtility.GetCurrentHttpContext();
            log.Device = httpContext.Request.Headers["device"].FirstOrDefault() ?? "web";
            log.RemoteIpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            log.Succeed = false;
            log.UserId = user.Id;
            log.UserName = user.Name;

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

                await _cacheService.RemoveCachesAsync(async (cancellToken) =>
                {
                    await _userRepository.UpdateAsync(new SysUser() { Id = user.Id, Status = 1 }, UpdatingProps<SysUser>(x => x.Status), cancellToken);
                }, _cacheService.ConcatCacheKey(CachingConsts.UserValidateInfoKeyPrefix, user.Id.ToString()));

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

            if (user.RoleIds.IsNullOrEmpty())
            {
                var problem = Problem(HttpStatusCode.Forbidden, "未分配任务角色，请联系管理员");
                log.Message = problem.Detail;
                log.StatusCode = problem.Status;
                _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);
                return problem;
            }

            await _cacheService.SetValidateInfoToCacheAsync(user);

            log.Message = "登录成功";
            log.StatusCode = (int)HttpStatusCode.Created;
            log.Succeed = true;
            _mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.Loginlog, log);

            return user;
        }

        public async Task<AppSrvResult> UpdatePasswordAsync(long id, UserChangePwdDto input)
        {
            var user = await _cacheService.GetUserValidateInfoFromCacheAsync(id);

            if (user == null)
                return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

            var md5OldPwdString = HashHelper.GetHashedString(HashType.MD5, input.OldPassword, user.Salt);
            if (!md5OldPwdString.EqualsIgnoreCase(user.Password))
                return Problem(HttpStatusCode.BadRequest, "旧密码输入错误");

            var newPwdString = HashHelper.GetHashedString(HashType.MD5, input.Password, user.Salt);

            await _userRepository.UpdateAsync(new SysUser { Id = user.Id, Password = newPwdString }, UpdatingProps<SysUser>(x => x.Password));

            return AppSrvResult();
        }

        public async Task<UserValidateDto> GetUserValidateInfoAsync(long id)
        {
            return await _cacheService.GetUserValidateInfoFromCacheAsync(id);
        }

        public async Task<UserInfoDto> GetUserInfoAsync(long id)
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
                return null;

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
    }
}
