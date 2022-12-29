﻿namespace Adnc.Usr.Application.Services;

public class AccountAppService : AbstractAppService, IAccountAppService
{
    private readonly IEfRepository<User> _userRepository;
    private readonly CacheService _cacheService;
    private readonly BloomFilterFactory _bloomFilterFactory;

    public AccountAppService(
        IEfRepository<User> userRepository
       , CacheService cacheService
       , BloomFilterFactory bloomFilterFactory)
    {
        _userRepository = userRepository;
        _cacheService = cacheService;
        _bloomFilterFactory = bloomFilterFactory;
    }

    public async Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input)
    {
        var accountsFilter = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfAccountsKey);
        var exists = await accountsFilter.ExistsAsync(input.Account.ToLower());
        if (!exists)
            return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

        var user = await _userRepository.FetchAsync(x => new
        {
            x.Id,
            x.Account,
            x.Password,
            x.Salt,
            x.Status,
            x.Email,
            x.Name,
            x.RoleIds
        }, x => x.Account == input.Account);

        if (user is null)
            return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

        var httpContext = InfraHelper.Accessor.GetCurrentHttpContext();

        var device = string.Empty;
        var ipAddress = string.Empty;
        if(httpContext is not null)
        {
            device = httpContext.Request.Headers["device"].FirstOrDefault() ?? "web";
            ipAddress = httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
        var channelWriter = ChannelAccessor<LoginLog>.Instance.Writer;
        var log = new LoginLog
        {
            Account = input.Account,
            Succeed = false,
            UserId = user.Id,
            UserName = user.Name,
            CreateTime = DateTime.Now,
            Device = device,
            RemoteIpAddress = ipAddress??string.Empty
        };

        if (user.Status != 1)
        {
            var problem = Problem(HttpStatusCode.TooManyRequests, "账号已锁定");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await channelWriter.WriteAsync(log);
            return problem;
        }

        // TODO
        var failLoginCount = await _cacheService.GetFailLoginCountByUserIdAsync(user.Id);// set fail count from cache
        if (failLoginCount >= 5)
        {
            var problem = Problem(HttpStatusCode.TooManyRequests, "连续登录失败次数超过5次，账号已锁定");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await channelWriter.WriteAsync(log);

            await _cacheService.RemoveCachesAsync(async (cancellToken) =>
            {
                await _userRepository.UpdateAsync(new User() { Id = user.Id, Status = 0 }, UpdatingProps<User>(x => x.Status), cancellToken);
            }, _cacheService.ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, user.Id));

            return problem;
        }

        if (InfraHelper.Encrypt.Md5(input.Password + user.Salt) != user.Password)
        {
            var problem = Problem(HttpStatusCode.BadRequest, "用户名或密码错误");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await _cacheService.SetFailLoginCountToCacheAsync(user.Id, ++failLoginCount);// set fail count to cache
            await channelWriter.WriteAsync(log);
            return problem;
        }

        if (user.RoleIds.IsNullOrEmpty())
        {
            var problem = Problem(HttpStatusCode.Forbidden, "未分配任务角色，请联系管理员");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await _cacheService.SetFailLoginCountToCacheAsync(user.Id, ++failLoginCount);// set fail count to cache
            await channelWriter.WriteAsync(log);
            return problem;
        }

        log.Message = "登录成功";
        log.StatusCode = (int)HttpStatusCode.Created;
        log.Succeed = true;
        await _cacheService.SetFailLoginCountToCacheAsync(user.Id, 0);// rest fail count to cache
        await channelWriter.WriteAsync(log);

        var userValidtedInfo = new UserValidatedInfoDto(user.Id, user.Account, user.Name, user.RoleIds, user.Status);
        await _cacheService.SetValidateInfoToCacheAsync(userValidtedInfo);

        return userValidtedInfo;
    }

    public async Task<AppSrvResult> ChangeUserValidateInfoExpiresDtAsync(long id)
    {
        await _cacheService.ChangeUserValidateInfoCacheExpiresDtAsync(id);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> UpdatePasswordAsync(long id, UserChangePwdDto input)
    {
        var user = await _userRepository.FetchAsync(x => new
        {
            x.Id,
            x.Salt,
            x.Password,
        }, x => x.Id == id);

        if (user is null)
            return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

        var md5OldPwdString = InfraHelper.Encrypt.Md5(input.OldPassword + user.Salt);
        if (!md5OldPwdString.EqualsIgnoreCase(user.Password))
            return Problem(HttpStatusCode.BadRequest, "旧密码输入错误");

        var newPwdString = InfraHelper.Encrypt.Md5(input.Password + user.Salt);

        await _userRepository.UpdateAsync(new User { Id = user.Id, Password = newPwdString }, UpdatingProps<User>(x => x.Password));

        return AppSrvResult();
    }
}