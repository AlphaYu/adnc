using Microsoft.AspNetCore.Http;

namespace Adnc.Demo.Admin.Application.Services;

public class UserService(IEfRepository<User> userRepository, IEfRepository<Role> roleRepository, IEfRepository<RoleUserRelation> roleUserRelationRepository
    , CacheService cacheService, /*BloomFilterFactory bloomFilterFactory,*/ IHttpContextAccessor httpContextAccessor)
    : AbstractAppService, IUserService
{
    public async Task<ServiceResult<IdDto>> CreateAsync(UserCreationDto input)
    {
        input.TrimStringFields();
        var exists = await userRepository.AnyAsync(x => x.Account == input.Account);
        if (exists)
        {
            return Problem(HttpStatusCode.BadRequest, "账号已经存在");
        }

        var user = Mapper.Map<User>(input, IdGenerater.GetNextId());
        user.Account = user.Account.ToLower();
        user.Salt = Random.Shared.Next(5, false);
        user.Password = InfraHelper.Encrypt.Md5(user.Password + user.Salt);

        //var cacheKey = cacheService.ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, user.Id);
        //var bloomFilterCacheKey = bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        //await bloomFilterCacheKey.AddAsync(cacheKey);

        //var bloomFilterAccount = bloomFilterFactory.Create(CachingConsts.BloomfilterOfAccountsKey);
        //await bloomFilterAccount.AddAsync(user.Account);

        if (input.RoleIds.IsNotNullOrEmpty())
        {
            var roleUsers = input.RoleIds.Select(x => new RoleUserRelation { Id = IdGenerater.GetNextId(), RoleId = x, UserId = user.Id });
            await roleUserRelationRepository.InsertRangeAsync(roleUsers);
        }

        await userRepository.InsertAsync(user);

        return new IdDto(user.Id);
    }

    public async Task<ServiceResult> UpdateAsync(long id, UserUpdationDto input)
    {
        input.TrimStringFields();

        var user = await userRepository.FetchAsync(x => x.Id == id, noTracking: false);
        if (user is null)
        {
            return Problem(HttpStatusCode.BadRequest, "账号不存在");
        }

        await roleUserRelationRepository.ExecuteDeleteAsync(x => x.UserId == id);
        if (input.RoleIds.IsNotNullOrEmpty())
        {
            var roleUsers = input.RoleIds.Select(x => new RoleUserRelation { Id = IdGenerater.GetNextId(), RoleId = x, UserId = id });
            await roleUserRelationRepository.InsertRangeAsync(roleUsers);
        }
        var newUser = Mapper.Map(input, user);
        await userRepository.UpdateAsync(newUser);

        return ServiceResult();
    }

    public async Task<ServiceResult> DeleteAsync(long[] ids)
    {
        await userRepository.ExecuteDeleteAsync(x => ids.Contains(x.Id));
        await roleUserRelationRepository.ExecuteDeleteAsync(x => ids.Contains(x.UserId));
        return ServiceResult();
    }

    public async Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions.IsNullOrEmpty())
        {
            return [];
        }

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
        {
            return [];
        }

        var roleIds = userBelongsRoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

        var allMenuCodes = await cacheService.GetAllRoleMenuCodesFromCacheAsync();

        var upperCodes = allMenuCodes.Where(x => roleIds.Contains(x.RoleId)).SelectMany(x => x.Perms.Select(y => y.ToUpper())).Distinct();
        if (upperCodes.IsNullOrEmpty())
        {
            return [];
        }

        var result = upperCodes.Intersect(requestPermissions.Select(x => x.ToUpper()));
        return result.ToList();
    }

    public async Task<UserDto?> GetAsync(long id)
    {
        var userEntity = await userRepository.FetchAsync(x => x.Id == id);
        if (userEntity is null)
        {
            return null;
        }

        var userDto = Mapper.Map<UserDto>(userEntity);
        var roleIds = await roleUserRelationRepository.Where(x => x.UserId == id).Select(x => x.RoleId).ToArrayAsync();
        userDto.RoleIds = roleIds;
        return userDto;
    }

    public async Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto input)
    {
        input.TrimStringFields();

        var whereExpression = ExpressionCreator
            .New<User>()
            .AndIf(input.Status is not null, x => x.Status == input.Status)
            .AndIf(input.DeptId is not null, x => x.DeptId == input.DeptId)
            .AndIf(input.CreateTime is not null && input.CreateTime.Length > 0, x => x.CreateTime >= input.CreateTime![0] && x.CreateTime <= input.CreateTime![1])
            .AndIf(input.Keywords.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Account, $"{input.Keywords}%")
            || EF.Functions.Like(x.Name, $"{input.Keywords}%")
            || EF.Functions.Like(x.Mobile, $"{input.Keywords}%"));

        var total = await userRepository.CountAsync(whereExpression);
        if (total == 0)
        {
            return new PageModelDto<UserDto>(input);
        }

        var userEntities = await userRepository
                                        .Where(whereExpression)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(input.SkipRows())
                                        .Take(input.PageSize)
                                        .ToListAsync();

        var userDtos = Mapper.Map<List<UserDto>>(userEntities);
        var deptsCahce = await cacheService.GetAllOrganizationsFromCacheAsync();
        foreach (var user in userDtos)
        {
            user.DeptName = deptsCahce.FirstOrDefault(x => x.Id == user.DeptId)?.Name ?? string.Empty;
        }

        return new PageModelDto<UserDto>(input, userDtos, total);
    }

    public async Task<UserProfileDto?> GetProfileAsync(long id)
    {
        var userEntity = await userRepository.FetchAsync(x => x.Id == id);
        if (userEntity is null)
        {
            return null;
        }

        var deptsCahce = await cacheService.GetAllOrganizationsFromCacheAsync();

        var roleQueryAble = roleRepository.GetAll();
        var roleUserQueryAble = roleUserRelationRepository.GetAll();
        var roleNames = await (from ru in roleUserQueryAble
                               join r in roleQueryAble on ru.RoleId equals r.Id
                               where ru.UserId == id
                               select r.Name).ToListAsync();

        var userProfileDto = Mapper.Map<UserProfileDto>(userEntity);
        userProfileDto.DeptName = deptsCahce.FirstOrDefault(x => x.Id == userEntity.DeptId)?.Name ?? string.Empty;
        userProfileDto.RoleNames = roleNames.ToString(",");

        return userProfileDto;
    }

    public async Task<ServiceResult> ChangeProfileAsync(long id, UserProfileUpdationDto input)
    {
        var exists = await userRepository.AnyAsync(x => x.Id == id);
        if (!exists)
        {
            return Problem(HttpStatusCode.NotFound, "用户不存在");
        }

        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(y => y.Name, input.Name).SetProperty(y => y.Gender, input.Gender));

        return ServiceResult();
    }

    public async Task<ServiceResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input)
    {
        input.TrimStringFields();

        var startTime = DateTime.Now;
        var device = httpContextAccessor.HttpContext?.Request.Headers["device"].FirstOrDefault() ?? "web";
        var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0.0.0.0";
        var channelWriter = ChannelAccessor<LoginLog>.Instance.Writer;
        var log = new LoginLog
        {
            Id = IdGenerater.GetNextId(),
            Account = input.Account,
            Succeed = false,
            UserId = 0,
            Name = string.Empty,
            CreateTime = DateTime.Now,
            Device = device,
            RemoteIpAddress = ipAddress
        };

        // 布隆过滤器演示代码
        //var accountsFilter = bloomFilterFactory.Create(CachingConsts.BloomfilterOfAccountsKey);
        //var exists = await accountsFilter.ExistsAsync(input.Account.ToLower());
        //if (!exists)
        //    return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

        var user = await userRepository.FetchAsync(x => new { x.Id, x.Status, x.Salt, x.Password, x.Account, x.Name }, y => y.Account == input.Account);
        if (user is null)
        {
            var problem = Problem(HttpStatusCode.BadRequest, "用户名或密码错误");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            await channelWriter.WriteAsync(log);
            return problem;
        }
        else
        {
            log.UserId = user.Id;
            log.Name = user.Name;
        }

        if (user.Status == false)
        {
            var problem = Problem(HttpStatusCode.TooManyRequests, "账号已锁定");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            await channelWriter.WriteAsync(log);
            return problem;
        }

        var failLoginCount = await cacheService.GetFailLoginCountByUserIdAsync(user.Id);// set fail count from cache
        if (failLoginCount >= 5)
        {
            var problem = Problem(HttpStatusCode.TooManyRequests, "连续登录失败次数超过5次，账号已锁定");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await channelWriter.WriteAsync(log);

            string[] needRemovedKeys = [cacheService.ConcatCacheKey(CachingConsts.UserFailCountKeyPrefix, user.Id), cacheService.ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, user.Id)];
            await cacheService.RemoveCachesAsync(async (cancellToken) =>
            {
                await userRepository.ExecuteUpdateAsync(x => x.Id == user.Id, setters => setters.SetProperty(y => y.Status, false), cancellToken);
            }, needRemovedKeys);

            return problem;
        }

        if (InfraHelper.Encrypt.Md5(input.Password + user.Salt) != user.Password)
        {
            var problem = Problem(HttpStatusCode.BadRequest, "用户名或密码错误");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            await cacheService.SetFailLoginCountToCacheAsync(user.Id, ++failLoginCount);// set fail count to cache
            await channelWriter.WriteAsync(log);
            return problem;
        }

        var roleUserQueryAble = roleUserRelationRepository.GetAll();
        var rolesQueryAble = roleRepository.GetAll();
        var roleInfos = await (from ru in roleUserQueryAble
                               join r in rolesQueryAble on ru.RoleId equals r.Id
                               where ru.UserId == user.Id
                               select new { ru.RoleId, RoleCode = r.Code, RoleName = r.Name }).ToListAsync();

        if (roleInfos.IsNullOrEmpty())
        {
            var problem = Problem(HttpStatusCode.Forbidden, "未分配任务角色，请联系管理员");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            await cacheService.SetFailLoginCountToCacheAsync(user.Id, ++failLoginCount);// set fail count to cache
            await channelWriter.WriteAsync(log);
            return problem;
        }

        log.Message = "登录成功";
        log.StatusCode = (int)HttpStatusCode.Created;
        log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
        log.Succeed = true;
        await cacheService.RemoveFailLoginCountToCacheAsync(user.Id);// remove fail count to cache
        await channelWriter.WriteAsync(log);

        var roleIds = roleInfos.Select(x => x.RoleId).ToArray();
        var roleCodes = roleInfos.Select(x => x.RoleCode).ToArray();
        var roleNames = roleInfos.Select(x => x.RoleName).ToArray();
        var userValidtedInfo = new UserValidatedInfoDto(user.Id, user.Account, user.Name, roleIds, roleCodes, roleNames, user.Status);
        await cacheService.SetValidateInfoToCacheAsync(userValidtedInfo);

        return userValidtedInfo;
    }

    public async Task<ServiceResult> ChangeUserValidateInfoExpiresDtAsync(long id)
    {
        await cacheService.ChangeUserValidateInfoCacheExpiresDtAsync(id);
        return ServiceResult();
    }

    public async Task<ServiceResult> UpdatePasswordAsync(long id, UserProfileChangePwdDto input)
    {
        input.TrimStringFields();
        var user = await userRepository.FetchAsync(x => new { x.Id, x.Salt, x.Password, }, x => x.Id == id);
        if (user is null)
        {
            return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");
        }

        var md5OldPwdString = InfraHelper.Encrypt.Md5(input.OldPassword + user.Salt);
        if (!md5OldPwdString.EqualsIgnoreCase(user.Password))
        {
            return Problem(HttpStatusCode.BadRequest, "旧密码输入错误");
        }

        var newPwdString = InfraHelper.Encrypt.Md5(input.ConfirmPassword + user.Salt);
        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(y => y.Password, newPwdString));

        return ServiceResult();
    }

    public async Task<ServiceResult> ResetPasswordAsync(long id, string password)
    {
        var user = await userRepository.FetchAsync(x => new { x.Id, x.Salt, x.Password, }, x => x.Id == id);
        if (user is null)
        {
            return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");
        }

        var newPwdString = InfraHelper.Encrypt.Md5(password + user.Salt);
        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(y => y.Password, newPwdString));

        return ServiceResult();
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(UserContext userContext)
    {
        var allRoleCodes = await cacheService.GetAllRoleMenuCodesFromCacheAsync();
        var userValidateInfo = await cacheService.GetUserValidateInfoFromCacheAsync(userContext.Id);
        if (userValidateInfo is null)
        {
            return null;
        }
        var perms = allRoleCodes.Where(x => userValidateInfo.RoleIds.Contains(x.RoleId) && x.Perms.IsNotNullOrEmpty()).SelectMany(x => x.Perms).Distinct();

        var userInfo = new UserInfoDto
        {
            Id = userContext.Id,
            Account = userContext.Account,
            Name = userContext.Name,
            // Avatar = string.Empty,
            Roles = userValidateInfo.RoleCodes,
            Perms = perms.ToArray()
        };

        return userInfo;
    }
}
