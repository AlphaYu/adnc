﻿namespace Adnc.Shared.WebApi.Authorization;

public class PermissionInfo
{
    public long RoleId { get; set; }
    public string MenuPerm { get; set; } = string.Empty;
}

public sealed class PermissionCacheHandler(ICacheProvider cacheProvider) : AbstractPermissionHandler
{
    private readonly ICacheProvider _cacheProvider = cacheProvider;

    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions == null || !requestPermissions.Any())
            return await Task.FromResult(true);

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
            return await Task.FromResult(false);

        var roleIds = userBelongsRoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

        var cache = await _cacheProvider.GetAsync<List<PermissionInfo>>(CacheConsts.MenuCodesCacheKey);

        if (cache == null || cache.IsNull)
            return await Task.FromResult(false);

        var upperCodes = cache.Value.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.MenuPerm.ToUpper());
        if (upperCodes == null || !upperCodes.Any())
            return await Task.FromResult(false);

        var result = upperCodes.Intersect(requestPermissions.Select(x => x.ToUpper()));

        return await Task.FromResult(result.Any());
    }
}