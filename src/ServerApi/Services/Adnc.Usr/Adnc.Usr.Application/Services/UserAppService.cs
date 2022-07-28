namespace Adnc.Usr.Application.Services;

public class UserAppService : AbstractAppService, IUserAppService
{
    private readonly IEfRepository<SysUser> _userRepository;
    private readonly IEfRepository<SysRole> _roleRepository;
    private readonly IEfRepository<SysMenu> _menuRepository;
    private readonly CacheService _cacheService;
    private readonly BloomFilterFactory _bloomFilterFactory;

    public UserAppService(
        IEfRepository<SysUser> userRepository
        , IEfRepository<SysRole> roleRepository
        , IEfRepository<SysMenu> menuRepository
        , CacheService cacheService
       , BloomFilterFactory bloomFilterFactory)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _cacheService = cacheService;
        _bloomFilterFactory = bloomFilterFactory;
    }

    public async Task<AppSrvResult<long>> CreateAsync(UserCreationDto input)
    {
        if (await _userRepository.AnyAsync(x => x.Account == input.Account))
            return Problem(HttpStatusCode.BadRequest, "账号已经存在");

        var user = Mapper.Map<SysUser>(input);
        user.Id = IdGenerater.GetNextId();
        user.Account = user.Account.ToLower();
        user.Salt = Random.Shared.Next(5, false);
        user.Password = InfraHelper.Security.MD5(user.Password + user.Salt);

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
        var user = Mapper.Map<SysUser>(input);

        user.Id = id;
        var updatingProps = UpdatingProps<SysUser>(x => x.Name, x => x.DeptId, x => x.Sex, x => x.Phone, x => x.Email, x => x.Birthday, x => x.Status);
        await _userRepository.UpdateAsync(user, updatingProps);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> SetRoleAsync(long id, UserSetRoleDto input)
    {
        var roleIdStr = input.RoleIds.IsNullOrEmpty() ? string.Empty : string.Join(",", input.RoleIds);
        await _userRepository.UpdateAsync(new SysUser() { Id = id, RoleIds = roleIdStr }, UpdatingProps<SysUser>(x => x.RoleIds));

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        await _userRepository.DeleteAsync(id);
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ChangeStatusAsync(long id, int status)
    {
        await _userRepository.UpdateAsync(new SysUser { Id = id, Status = status }, UpdatingProps<SysUser>(x => x.Status));
        return AppSrvResult();
    }

    public async Task<AppSrvResult> ChangeStatusAsync(IEnumerable<long> ids, int status)
    {
        await _userRepository.UpdateRangeAsync(u => ids.Contains(u.Id), u => new SysUser { Status = status });
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
        var whereExpression = ExpressionCreator
                                            .New<SysUser>()
                                            .AndIf(search.Account.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Account, $"%{search.Account.Trim()}%"))
                                            .AndIf(search.Name.IsNotNullOrWhiteSpace(), x => EF.Functions.Like(x.Name, $"%{search.Name.Trim()}%"));

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
            var depts = (await _cacheService.GetAllDeptsFromCacheAsync()).Where(x => deptIds.Contains(x.Id)).Select(d => new { d.Id, d.FullName });
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

        var xdata = await _cacheService.GetDeptSimpleTreeListAsync();
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
}