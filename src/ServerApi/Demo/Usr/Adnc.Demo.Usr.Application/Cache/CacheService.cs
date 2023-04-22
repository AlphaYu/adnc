namespace Adnc.Demo.Usr.Application.Cache;

public sealed class CacheService : AbstractCacheService, ICachePreheatable
{
    private readonly Lazy<IOptions<JWTOptions>> _jwtConfig;

    public CacheService(
        Lazy<ICacheProvider> cacheProvider,
        Lazy<IServiceProvider> serviceProvider,
        Lazy<IOptions<JWTOptions>> jwtConfig)
        : base(cacheProvider, serviceProvider)
    {
        _jwtConfig = jwtConfig;
    }

    public override async Task PreheatAsync()
    {
        await GetAllOrganizationsFromCacheAsync();
        await GetAllRelationsFromCacheAsync();
        await GetAllMenusFromCacheAsync();
        await GetAllRolesFromCacheAsync();
        await GetAllMenuCodesFromCacheAsync();
        await GetOrganizationsSimpleTreeListAsync();
    }

    internal int GetRefreshTokenExpires() =>
        _jwtConfig.Value.Value.RefreshTokenExpire * 60 + _jwtConfig.Value.Value.ClockSkew;

    internal async Task SetFailLoginCountToCacheAsync(long id, int count)
    {
        var cacheKey = ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, $"FailCount_{id}");
        await CacheProvider.Value.SetAsync(cacheKey, count, TimeSpan.FromSeconds(GetRefreshTokenExpires()));
    }

    internal async Task SetValidateInfoToCacheAsync(UserValidatedInfoDto value)
    {
        var cacheKey = ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, value.Id);
        await CacheProvider.Value.SetAsync(cacheKey, value, TimeSpan.FromSeconds(GetRefreshTokenExpires()));
    }

    internal async Task<UserValidatedInfoDto> GetUserValidateInfoFromCacheAsync(long id)
    {
        var cacheKey = ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, id.ToString());
        //var cacheValue = await CacheProvider.Value.GetAsync(cacheKey, async () =>
        //{
        //    using var scope = ServiceProvider.Value.CreateScope();
        //    var userRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<SysUser>>();
        //    return await userRepository.FetchAsync(x => new UserValidatedInfoDto(x.Id, x.Account, x.Name, x.RoleIds, x.Status, x.Password), x => x.Id == Id && x.Status == 1);
        //}, GetRefreshTokenExpires());
        var cacheValue = await CacheProvider.Value.GetAsync<UserValidatedInfoDto>(cacheKey);
        return cacheValue.Value;
    }

    internal async Task ChangeUserValidateInfoCacheExpiresDtAsync(long id)
    {
        var cacheKey = ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, id);
        await CacheProvider.Value.KeyExpireAsync(new string[] { cacheKey }, GetRefreshTokenExpires());
    }

    internal async Task<List<OrganizationDto>> GetAllOrganizationsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.DetpListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var orgRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Organization>>();
            var allOrganizations = await orgRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<OrganizationDto>>(allOrganizations);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RelationDto>> GetAllRelationsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.MenuRelationCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var relationRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<RoleRelation>>();
            var allRelations = await relationRepository.GetAll(writeDb: true).ToListAsync();
            return Mapper.Value.Map<List<RelationDto>>(allRelations);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<MenuDto>> GetAllMenusFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.MenuListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var menuRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Menu>>();
            var allMenus = await menuRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<MenuDto>>(allMenus);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleDto>> GetAllRolesFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.RoleListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var roleRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<Role>>();
            var allRoles = await roleRepository.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<RoleDto>>(allRoles);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleMenuCodesDto>> GetAllMenuCodesFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.MenuCodesCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var relationRepository = scope.ServiceProvider.GetRequiredService<IEfRepository<RoleRelation>>();
            var allMenus = await relationRepository.GetAll(writeDb: true)
                                                                            .Where(x => x.Menu.Status)
                                                                            .Select(x => new RoleMenuCodesDto { RoleId = x.RoleId, Code = x.Menu.Code })
                                                                            .ToListAsync();
            return allMenus.Distinct().ToList();
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<OrganizationSimpleTreeDto>> GetOrganizationsSimpleTreeListAsync()
    {
        var result = new List<OrganizationSimpleTreeDto>();

        var cacheValue = await CacheProvider.Value.GetAsync<List<OrganizationSimpleTreeDto>>(CachingConsts.DetpSimpleTreeListCacheKey);
        if (cacheValue.HasValue)
            return cacheValue.Value;

        var Organizations = await GetAllOrganizationsFromCacheAsync();

        if (Organizations.IsNullOrEmpty())
            return result;

        var roots = Organizations.Where(d => d.Pid == 0)
                                    .OrderBy(d => d.Ordinal)
                                    .Select(x => new OrganizationSimpleTreeDto { Id = x.Id, Label = x.SimpleName })
                                    .ToList();
        foreach (var node in roots)
        {
            GetChildren(node, Organizations);
            result.Add(node);
        }

        void GetChildren(OrganizationSimpleTreeDto currentNode, List<OrganizationDto> Organizations)
        {
            var childrenNodes = Organizations.Where(d => d.Pid == currentNode.Id)
                                                       .OrderBy(d => d.Ordinal)
                                                       .Select(x => new OrganizationSimpleTreeDto() { Id = x.Id, Label = x.SimpleName })
                                                       .ToList();
            if (childrenNodes.IsNotNullOrEmpty())
            {
                currentNode.Children.AddRange(childrenNodes);
                foreach (var node in childrenNodes)
                {
                    GetChildren(node, Organizations);
                }
            }
        }

        await CacheProvider.Value.SetAsync(CachingConsts.DetpSimpleTreeListCacheKey, result, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return result;
    }

    internal async Task<int> GetFailLoginCountByUserIdAsync(long id)
    {
        var cacheKey = ConcatCacheKey(CachingConsts.UserValidatedInfoKeyPrefix, $"FailCount_{id}");
        var cacheValue = await CacheProvider.Value.GetAsync<int>(cacheKey);
        return cacheValue.Value;
    }
}