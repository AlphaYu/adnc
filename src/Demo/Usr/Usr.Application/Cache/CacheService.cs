namespace Adnc.Demo.Usr.Application.Cache;

public sealed class CacheService(Lazy<ICacheProvider> cacheProvider, Lazy<IServiceProvider> serviceProvider, Lazy<IConfiguration> configuration)
    : AbstractCacheService(cacheProvider, serviceProvider), ICachePreheatable
{
    public override async Task PreheatAsync()
    {
        await GetAllOrganizationsFromCacheAsync();
        await GetAllRoleUserRelationsFromCacheAsync();
        await GetAllMenusFromCacheAsync();
        await GetAllRolesFromCacheAsync();
        await GetAllRoleMenuCodesFromCacheAsync();
        await GetOrganizationsSimpleTreeListAsync();
    }

    internal int GetRefreshTokenExpires()
    {
        var refreshTokenExpire = configuration.Value.GetValue<int>($"{NodeConsts.JWT}:RefreshTokenExpire");
        var clockSkew = configuration.Value.GetValue<int>($"{NodeConsts.JWT}:ClockSkew");
        return refreshTokenExpire + clockSkew;
    }

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
            var orgRepo = scope.ServiceProvider.GetRequiredService<IEfRepository<Organization>>();
            var allOrganizations = await orgRepo.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<OrganizationDto>>(allOrganizations);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleMenuRelationDto>> GetAllRoleUserRelationsFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.MenuRelationCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var roleMenuRepo = scope.ServiceProvider.GetRequiredService<IEfRepository<RoleMenuRelation>>();
            var allRelations = await roleMenuRepo.GetAll(writeDb: true).ToListAsync();
            return Mapper.Value.Map<List<RoleMenuRelationDto>>(allRelations);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<MenuDto>> GetAllMenusFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.MenuListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var menuRepo = scope.ServiceProvider.GetRequiredService<IEfRepository<Menu>>();
            var allMenus = await menuRepo.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<MenuDto>>(allMenus);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleDto>> GetAllRolesFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.RoleListCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var roleRepo = scope.ServiceProvider.GetRequiredService<IEfRepository<Role>>();
            var allRoles = await roleRepo.GetAll(writeDb: true).OrderBy(x => x.Ordinal).ToListAsync();
            return Mapper.Value.Map<List<RoleDto>>(allRoles);
        }, TimeSpan.FromSeconds(GeneralConsts.OneYear));

        return cahceValue.Value;
    }

    internal async Task<List<RoleMenuCodesDto>> GetAllRoleMenuCodesFromCacheAsync()
    {
        var cahceValue = await CacheProvider.Value.GetAsync(CachingConsts.MenuCodesCacheKey, async () =>
        {
            using var scope = ServiceProvider.Value.CreateScope();
            var menuRepo = scope.ServiceProvider.GetRequiredService<IEfRepository<Menu>>();
            var roleMenuRepo = scope.ServiceProvider.GetRequiredService<IEfRepository<RoleMenuRelation>>();

            var menuQueryAble = menuRepo.GetAll().Where(x => x.Status);
            var roleMenuQueryAble = roleMenuRepo.GetAll();

            var roleMenus = await (from r in roleMenuQueryAble
                                   join m in menuQueryAble on r.MenuId equals m.Id
                                   select new RoleMenuCodesDto { RoleId = r.RoleId, Code = m.Code }
                                ).ToListAsync();

            return roleMenus;
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

        var roots = Organizations.Where(d => d.Pid == 0).OrderBy(d => d.Ordinal).Select(x => new OrganizationSimpleTreeDto { Id = x.Id, Label = x.SimpleName }).ToList();
        foreach (var node in roots)
        {
            GetChildren(node, Organizations);
            result.Add(node);
        }

        void GetChildren(OrganizationSimpleTreeDto currentNode, List<OrganizationDto> Organizations)
        {
            var childrenNodes = Organizations.Where(d => d.Pid == currentNode.Id).OrderBy(d => d.Ordinal).Select(x => new OrganizationSimpleTreeDto() { Id = x.Id, Label = x.SimpleName }).ToList();
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