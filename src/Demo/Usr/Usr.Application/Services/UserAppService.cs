using Microsoft.AspNetCore.Http;

namespace Adnc.Demo.Usr.Application.Services;

public class UserAppService(
        IEfRepository<User> userRepository
        , IEfRepository<Role> roleRepository
        , IEfRepository<RoleUserRelation> roleUserRelationRepository
        , CacheService cacheService
        //, BloomFilterFactory bloomFilterFactory
        , IHttpContextAccessor httpContextAccessor) : AbstractAppService, IUserAppService
{
    public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
    {
        input.TrimStringFields();
        var exists = await userRepository.AnyAsync(x => x.Account == input.Account);
        if (exists)
            return Problem(HttpStatusCode.BadRequest, "账号已经存在");

        var user = Mapper.Map<User>(input, IdGenerater.GetNextId());
        user.Account = user.Account.ToLower();
        user.Salt = Random.Shared.Next(5, false);
        user.Password = InfraHelper.Encrypt.Md5(user.Password + user.Salt);

        //var cacheKey = cacheService.ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, user.Id);
        //var bloomFilterCacheKey = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfCacheKey);
        //await bloomFilterCacheKey.AddAsync(cacheKey);

        //var bloomFilterAccount = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfAccountsKey);
        //await bloomFilterAccount.AddAsync(user.Account);
        if (input.RoleIds.IsNotNullOrEmpty())
        {
            var roleUsers = input.RoleIds.Select(x => new RoleUserRelation { Id = IdGenerater.GetNextId(), RoleId = x, UserId = user.Id });
            await roleUserRelationRepository.InsertRangeAsync(roleUsers);
        }

        await userRepository.InsertAsync(user);

        return user.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, UserUpdationDto input)
    {
        input.TrimStringFields();
        await roleUserRelationRepository.ExecuteDeleteAsync(x => x.UserId == id);

        if (input.RoleIds.IsNotNullOrEmpty())
        {
            var roleUsers = input.RoleIds.Select(x => new RoleUserRelation { Id = IdGenerater.GetNextId(), RoleId = x, UserId = id });
            await roleUserRelationRepository.InsertRangeAsync(roleUsers);
        }

        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters
            .SetProperty(x => x.Name, input.Name)
            .SetProperty(x => x.DeptId, input.DeptId)
            .SetProperty(x => x.Gender, input.Gender)
            .SetProperty(x => x.Mobile, input.Mobile)
            .SetProperty(x => x.Email, input.Email)
            .SetProperty(x => x.Birthday, input.Birthday)
            .SetProperty(x => x.Status, input.Status));

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long[] ids)
    {
        await userRepository.ExecuteDeleteAsync(x=>ids.Contains(x.Id));
        await roleUserRelationRepository.ExecuteDeleteAsync(x => ids.Contains(x.UserId));
        return AppSrvResult();
    }

    public async Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions.IsNullOrEmpty())
            return ["allow"];

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
            return [];

        var roleIds = userBelongsRoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

        var allMenuCodes = await cacheService.GetAllRoleMenusFromCacheAsync();

        var upperCodes = allMenuCodes?.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.MenuPerm.ToUpper()) ?? [];
        if (upperCodes.IsNullOrEmpty())
            return [];

        var result = upperCodes.Intersect(requestPermissions.Select(x => x.ToUpper()));
        return result.ToList();
    }

    public async Task<UserDto> GetAsync(long id)
    {
        var userEntity =await userRepository.FetchAsync(x=>x.Id == id);
        var userDto = Mapper.Map<UserDto>(userEntity);
        var roleIds = await roleUserRelationRepository.Where(x => x.UserId == id).Select(x => x.RoleId).ToArrayAsync();
        userDto.RoleIds = roleIds;
        return userDto;
    }

    public async Task<PageModelDto<UserDto>> GetPagedAsync(UserSearchPagedDto search)
    {
        search.TrimStringFields();
        var whereExpression = ExpressionCreator
                                            .New<User>()
                                            .AndIf(search.Status is not null, x => x.Status == search.Status)
                                            .AndIf(search.DeptId is not null, x => x.DeptId == search.DeptId)
                                            .AndIf(search.CreateTime is not null && search.CreateTime.Length > 0, x => x.CreateTime >= search.CreateTime[0] && x.CreateTime <= search.CreateTime[1])
                                            .AndIf(search.Keywords.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Account, $"{search.Keywords}%")
                                            || EF.Functions.Like(x.Name, $"{search.Keywords}%")
                                            || EF.Functions.Like(x.Mobile, $"{search.Keywords}%"));

        var total = await userRepository.CountAsync(whereExpression);
        if (total == 0)
            return new PageModelDto<UserDto>(search);

        var userEntities = await userRepository
                                        .Where(whereExpression)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(search.SkipRows())
                                        .Take(search.PageSize)
                                        .ToListAsync();

        var userDtos = Mapper.Map<List<UserDto>>(userEntities);
        var deptsCahce = await cacheService.GetAllOrganizationsFromCacheAsync();
        foreach (var user in userDtos)
        {
            user.DeptName = deptsCahce.FirstOrDefault(x => x.Id == user.DeptId)?.Name ?? string.Empty;
        }

        return new PageModelDto<UserDto>(search, userDtos, total);
    }

    public async Task<UserProfileDto?> GetProfileAsync(long id)
    {
        var userEntity = await userRepository.FetchAsync(x => x.Id == id);
        if (userEntity is null)
            return null;

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

    public async Task<AppSrvResult> ChangeProfileAsync(long id, UserProfileUpdationDto input)
    {
        var exists = await userRepository.AnyAsync(x => x.Id == id);
        if (!exists)
            return Problem(HttpStatusCode.NotFound, "用户不存在");

        await userRepository.ExecuteUpdateAsync(x => x.Id == x.Id, setters => setters.SetProperty(y => y.Name, input.Name).SetProperty(y => y.Gender, input.Gender));

        return AppSrvResult();
    }

    public async Task<AppSrvResult<UserValidatedInfoDto>> LoginAsync(UserLoginDto input)
    {
        input.TrimStringFields();
        //var accountsFilter = _bloomFilterFactory.Create(CachingConsts.BloomfilterOfAccountsKey);
        //var exists = await accountsFilter.ExistsAsync(input.Account.ToLower());
        //if (!exists)
        //    return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

        var user = await userRepository.FetchAsync(x => x.Account == input.Account);
        if (user is null)
            return Problem(HttpStatusCode.BadRequest, "用户名或密码错误");

        var device = httpContextAccessor.HttpContext?.Request.Headers["device"].FirstOrDefault() ?? "web";
        var ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "0.0.0.0";
        var channelWriter = Accessor<LoginLog>.Instance.Writer;
        var log = new LoginLog
        {
            Id = IdGenerater.GetNextId(),
            Account = input.Account,
            Succeed = false,
            UserId = user.Id,
            UserName = user.Name,
            CreateTime = DateTime.Now,
            Device = device,
            RemoteIpAddress = ipAddress
        };

        if (user.Status==false)
        {
            var problem = Problem(HttpStatusCode.TooManyRequests, "账号已锁定");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await channelWriter.WriteAsync(log);
            return problem;
        }

        // TODO
        var failLoginCount = await cacheService.GetFailLoginCountByUserIdAsync(user.Id);// set fail count from cache
        if (failLoginCount >= 5)
        {
            var problem = Problem(HttpStatusCode.TooManyRequests, "连续登录失败次数超过5次，账号已锁定");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
            await channelWriter.WriteAsync(log);

            await cacheService.RemoveCachesAsync(async (cancellToken) =>
            {
                await userRepository.ExecuteUpdateAsync(x => x.Id == user.Id, setters => setters.SetProperty(y => y.Status, false), cancellToken);
            }, cacheService.ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, user.Id));

            return problem;
        }

        if (InfraHelper.Encrypt.Md5(input.Password + user.Salt) != user.Password)
        {
            var problem = Problem(HttpStatusCode.BadRequest, "用户名或密码错误");
            log.Message = problem.Detail;
            log.StatusCode = problem.Status;
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
            await cacheService.SetFailLoginCountToCacheAsync(user.Id, ++failLoginCount);// set fail count to cache
            await channelWriter.WriteAsync(log);
            return problem;
        }

        log.Message = "登录成功";
        log.StatusCode = (int)HttpStatusCode.Created;
        log.Succeed = true;
        await cacheService.SetFailLoginCountToCacheAsync(user.Id, 0);// rest fail count to cache
        await channelWriter.WriteAsync(log);

        var roleIds = roleInfos.Select(x => x.RoleId).ToArray();
        var roleCodes = roleInfos.Select(x => x.RoleCode).ToArray();
        var roleNames = roleInfos.Select(x => x.RoleName).ToArray();
        var userValidtedInfo = new UserValidatedInfoDto(user.Id, user.Account, user.Name, roleIds, roleCodes, roleNames, user.Status);
        await cacheService.SetValidateInfoToCacheAsync(userValidtedInfo);

        return userValidtedInfo;
    }

    public async Task<AppSrvResult> ChangeUserValidateInfoExpiresDtAsync(long id)
    {
        await cacheService.ChangeUserValidateInfoCacheExpiresDtAsync(id);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> UpdatePasswordAsync(long id, UserProfileChangePwdDto input)
    {
        input.TrimStringFields();
        var user = await userRepository.FetchAsync(x => new { x.Id, x.Salt, x.Password, }, x => x.Id == id);
        if (user is null)
            return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

        var md5OldPwdString = InfraHelper.Encrypt.Md5(input.OldPassword + user.Salt);
        if (!md5OldPwdString.EqualsIgnoreCase(user.Password))
            return Problem(HttpStatusCode.BadRequest, "旧密码输入错误");

        var newPwdString = InfraHelper.Encrypt.Md5(input.ConfirmPassword + user.Salt);
        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(y => y.Password, newPwdString));

        return AppSrvResult();
    }

    public async Task<AppSrvResult> ResetPasswordAsync(long id, string password)
    {
        var user = await userRepository.FetchAsync(x => new { x.Id, x.Salt, x.Password, }, x => x.Id == id);
        if (user is null)
            return Problem(HttpStatusCode.NotFound, "用户不存在,参数信息不完整");

        var newPwdString = InfraHelper.Encrypt.Md5(password + user.Salt);
        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(y => y.Password, newPwdString));

        return AppSrvResult();
    }

    public async Task<UserInfoDto> GetUserInfoAsync(UserContext userContext)
    {
        //所有菜单角色关系
        var allRoleMenus = await cacheService.GetAllRoleMenusFromCacheAsync();
        //角色拥有的菜单Ids
        var roleIds = userContext.RoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLong());
        var perms = allRoleMenus.Where(x => roleIds.Contains(x.RoleId) && x.MenuPerm.IsNotNullOrEmpty()).Select(x => x.MenuPerm).Distinct();
        var userValidateInfo = await cacheService.GetUserValidateInfoFromCacheAsync(userContext.Id);

        var userInfo = new UserInfoDto
        {
            Id = userContext.Id,
            Account = userContext.Account,
            Name = userContext.Name,
            Avatar = string.Empty,
            Roles = userValidateInfo.RoleCodes,
            Perms = perms.ToArray()
        };

        return userInfo;
    }
}