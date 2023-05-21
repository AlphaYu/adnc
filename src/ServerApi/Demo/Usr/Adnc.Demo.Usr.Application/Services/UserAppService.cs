using Microsoft.AspNetCore.Http;

namespace Adnc.Demo.Usr.Application.Services;

public class UserAppService : AbstractAppService, IUserAppService
{
    private readonly IEfRepository<User> _userRepository;
    private readonly IEfRepository<Role> _roleRepository;
    private readonly IEfRepository<Menu> _menuRepository;
    private readonly CacheService _cacheService;
    private readonly BloomFilterFactory _bloomFilterFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAppService(
        IEfRepository<User> userRepository
        , IEfRepository<Role> roleRepository
        , IEfRepository<Menu> menuRepository
        , CacheService cacheService
        , BloomFilterFactory bloomFilterFactory
        , IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _cacheService = cacheService;
        _bloomFilterFactory = bloomFilterFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
    {
        input.TrimStringFields();
        if (await _userRepository.AnyAsync(x => x.Account == input.Account))
            return Problem(HttpStatusCode.BadRequest, "账号已经存在");

        var user = Mapper.Map<User>(input);
        user.Id = IdGenerater.GetNextId();
        user.Account = user.Account.ToLower();
        user.Salt = Random.Shared.Next(5, false);
        user.Password = InfraHelper.Encrypt.Md5(user.Password + user.Salt);

        var cacheKey = _cacheService.ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, user.Id);
        var bloomFilterCacheKey = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        await bloomFilterCacheKey.AddAsync(cacheKey);

        var bloomFilterAccount = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfAccountsKey);
        await bloomFilterAccount.AddAsync(user.Account);

        await _userRepository.InsertAsync(user);

        return user.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, UserUpdationDto input)
    {
        input.TrimStringFields();
        var user = Mapper.Map<User>(input);

        user.Id = id;
        var updatingProps = UpdatingProps<User>(x => x.Name, x => x.DeptId, x => x.Sex, x => x.Phone, x => x.Email, x => x.Birthday, x => x.Status);
        await _userRepository.UpdateAsync(user, updatingProps);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> SetRoleAsync(long id, UserSetRoleDto input)
    {
        var roleIdStr = input.RoleIds.IsNullOrEmpty() ? string.Empty : string.Join(",", input.RoleIds);
        await _userRepository.UpdateAsync(new User() { Id = id, RoleIds = roleIdStr }, UpdatingProps<User>(x => x.RoleIds));

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        await _userRepository.DeleteAsync(id);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ChangeStatusAsync(long id, int status)
    {
        await _userRepository.UpdateAsync(new User { Id = id, Status = status }, UpdatingProps<User>(x => x.Status));
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ChangeStatusAsync(IEnumerable<long> ids, int status)
    {
        await _userRepository.UpdateRangeAsync(u => ids.Contains(u.Id), u => new User { Status = status });
        return AppSrvResult();
    }

    public async Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (userBelongsRoleIds.IsNullOrWhiteSpace())
            return default;

        if (requestPermissions.IsNullOrEmpty())
            return new List<string> { "allow" };

        var roleIds = userBelongsRoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

        var allMenuCodes = await _cacheService.GetAllMenuCodesFromCacheAsync();

        var upperCodes = allMenuCodes?.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.Code.ToUpper());
        if (upperCodes.IsNullOrEmpty())
            return default;

        var result = upperCodes.Intersect(requestPermissions.Select(x => x.ToUpper()));
        return result.ToList();
    }

    public async Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto search)
    {
        search.TrimStringFields();
        var whereExpression = ExpressionCreator
                                            .New<User>()
                                            .AndIf(search.Account.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Account, $"%{search.Account}%"))
                                            .AndIf(search.Name.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"%{search.Name}%"));

        var total = await _userRepository.CountAsync(whereExpression);
        if (total == 0)
            return new PageModelDto<UserDto>(search);

        var entities = await _userRepository
                                        .Where(whereExpression)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(search.SkipRows())
                                        .Take(search.PageSize)
                                        .ToListAsync();

        var userDtos = Mapper.Map<List<UserDto>>(entities);
        if (userDtos.IsNotNullOrEmpty())
        {
            var deptIds = userDtos.Where(d => d.DeptId is not null).Select(d => d.DeptId).Distinct();
            var depts = (await _cacheService.GetAllOrganizationsFromCacheAsync()).Where(x => deptIds.Contains(x.Id)).Select(d => new { d.Id, d.FullName });
            var roles = (await _cacheService.GetAllRolesFromCacheAsync()).Select(r => new { r.Id, r.Name });
            foreach (var user in userDtos)
            {
                user.DeptName = depts.FirstOrDefault(x => x.Id == user.DeptId)?.FullName;

                var roleIds = user.RoleIds.IsNullOrWhiteSpace()
                                        ? new List<long>()
                                        : user.RoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x))
                                        ;
                user.RoleNames = roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name).ToString(",");
            }
        }

        var xdata = await _cacheService.GetOrganizationsSimpleTreeListAsync();
        return new PageModelDto<UserDto>(search, userDtos, total, xdata);
    }

    public async Task<UserInfoDto> GetUserInfoAsync(long id)
    {
        var userProfile = await _userRepository.FetchAsync(u => new UserProfileDto
        {
            Account = u.Account,
            Avatar = u.Avatar,
            Birthday = u.Birthday,
            DeptId = u.DeptId,
            DeptFullName = u.Dept.FullName,
            Email = u.Email,
            Name = u.Name,
            Phone = u.Phone,
            RoleIds = u.RoleIds,
            Sex = u.Sex,
            Status = u.Status
        }, x => x.Id == id);

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
            if (roleMenus.IsNotNullOrEmpty())
                userInfoDto.Permissions.AddRange(roleMenus.Select(x => x.Url).Distinct());
        }

        return userInfoDto;
    }

    public async Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input)
    {
        input.TrimStringFields();
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

        var httpContext = _httpContextAccessor.HttpContext;

        var device = string.Empty;
        var ipAddress = string.Empty;
        if (httpContext is not null)
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
            RemoteIpAddress = ipAddress ?? string.Empty
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
        input.TrimStringFields();
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