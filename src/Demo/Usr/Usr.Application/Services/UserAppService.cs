using Microsoft.AspNetCore.Http;

namespace Adnc.Demo.Usr.Application.Services;

public class UserAppService(
    IEfRepository<User> userRepository
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

        await userRepository.InsertAsync(user);

        return user.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, UserUpdationDto input)
    {
        input.TrimStringFields();
        //var user = Mapper.Map<User>(input, id);
        //var updatingProps = UpdatingProps<User>(x => x.Name, x => x.DeptId, x => x.Gender, x => x.Phone, x => x.Email, x => x.Birthday, x => x.Status);
        //await userRepository.UpdateAsync(user, updatingProps);
        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters
            .SetProperty(x => x.Name, input.Name)
            .SetProperty(x => x.DeptId, input.DeptId)
            .SetProperty(x => x.Gender, input.Sex)
            .SetProperty(x => x.Phone, input.Phone)
            .SetProperty(x => x.Email, input.Email)
            .SetProperty(x => x.Birthday, input.Birthday)
            .SetProperty(x => x.Status, input.Status));

        return AppSrvResult();
    }

    public async Task<AppSrvResult> SetRoleAsync(long id, UserSetRoleDto input)
    {
        var exists = await userRepository.AnyAsync(x => x.Id == id);
        if (!exists)
            return AppSrvResult();

        await roleUserRelationRepository.ExecuteDeleteAsync(x => x.UserId == id);

        if (input.RoleIds.IsNotNullOrEmpty())
        {
            var roleUserRelations = input.RoleIds.Select(x => new RoleUserRelation { RoleId = x, UserId = id });
            await roleUserRelationRepository.InsertRangeAsync(roleUserRelations);
        }

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        await userRepository.DeleteAsync(id);
        await roleUserRelationRepository.ExecuteDeleteAsync(x => x.UserId == id);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ChangeStatusAsync(long id, int status)
    {
        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(x => x.Status, status));
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ChangeStatusAsync(long[] ids, int status)
    {
        await userRepository.ExecuteUpdateAsync(u => ids.Contains(u.Id), setters => setters.SetProperty(x => x.Status, status));
        return AppSrvResult();
    }

    public async Task<List<string>> GetPermissionsAsync(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions.IsNullOrEmpty())
            return ["allow"];

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
            return [];

        var roleIds = userBelongsRoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

        var allMenuCodes = await cacheService.GetAllRoleMenuCodesFromCacheAsync();

        var upperCodes = allMenuCodes?.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.Code.ToUpper()) ?? [];
        if (upperCodes.IsNullOrEmpty())
            return [];

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

        var total = await userRepository.CountAsync(whereExpression);
        if (total == 0)
            return new PageModelDto<UserDto>(search);

        var userEntities = await userRepository
                                        .Where(whereExpression)
                                        .OrderByDescending(x => x.Id)
                                        .Skip(search.SkipRows())
                                        .Take(search.PageSize)
                                        .ToListAsync();

        var roleUserEntities = await roleUserRelationRepository
                                            .Where(x => userEntities.Select(u => u.Id).Contains(x.UserId))
                                            .ToListAsync();

        var deptsCahce = await cacheService.GetAllOrganizationsFromCacheAsync();
        var rolesCache = await cacheService.GetAllRolesFromCacheAsync();

        var userDtos = Mapper.Map<List<UserDto>>(userEntities);
        foreach (var user in userDtos)
        {
            user.DeptName = deptsCahce.FirstOrDefault(x => x.Id == user.DeptId)?.FullName ?? string.Empty;
            var belongRoleIds = roleUserEntities.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();
            user.RoleNames = rolesCache.Where(x => belongRoleIds.Contains(x.Id)).Select(x => x.Name).ToArray();
        }

        return new PageModelDto<UserDto>(search, userDtos, total, deptsCahce);
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(long id)
    {

        var userEntity = await userRepository.FetchAsync(x => x.Id == id);
        if (userEntity is null)
            return null;

        var roleIds = await roleUserRelationRepository.Where(x => x.UserId == id).Select(x => x.RoleId).ToListAsync();
        var deptsCahce = await cacheService.GetAllOrganizationsFromCacheAsync();
        var rolesCache = await cacheService.GetAllRolesFromCacheAsync();


        var userProfileDto = Mapper.Map<UserProfileDto>(userEntity);
        userProfileDto.DeptFullName = deptsCahce.FirstOrDefault(x => x.Id == userEntity.DeptId)?.FullName ?? string.Empty;
        userProfileDto.RoleNames = rolesCache.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name).ToString(",");

        return userProfileDto;
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

        if (user.Status != 1)
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
                await userRepository.ExecuteUpdateAsync(x => x.Id == user.Id, setters => setters.SetProperty(y => y.Status, 0), cancellToken);
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

        var roleIds = await roleUserRelationRepository.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToListAsync();
        if (roleIds.IsNullOrEmpty())
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

        var userValidtedInfo = new UserValidatedInfoDto(user.Id, user.Account, user.Name, roleIds.ToString(","), user.Status);
        await cacheService.SetValidateInfoToCacheAsync(userValidtedInfo);

        return userValidtedInfo;
    }

    public async Task<AppSrvResult> ChangeUserValidateInfoExpiresDtAsync(long id)
    {
        await cacheService.ChangeUserValidateInfoCacheExpiresDtAsync(id);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> UpdatePasswordAsync(long id, UserChangePwdDto input)
    {
        input.TrimStringFields();
        var user = await userRepository.FetchAsync(x => new
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

        await userRepository.ExecuteUpdateAsync(x => x.Id == id, setters => setters.SetProperty(y => y.Password, newPwdString));

        return AppSrvResult();
    }
}