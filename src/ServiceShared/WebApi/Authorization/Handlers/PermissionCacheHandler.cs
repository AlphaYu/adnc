namespace Adnc.Shared.WebApi.Authorization.Handlers;

public class PermissionInfo
{
    public long RoleId { get; set; }
    public string[] Perms { get; set; } = [];
}

public sealed class PermissionCacheHandler(ICacheProvider cacheProvider) : AbstractPermissionHandler
{
    protected override async Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds)
    {
        if (requestPermissions == null || !requestPermissions.Any())
        {
            return await Task.FromResult(true);
        }

        var permStr = string.Join("", requestPermissions);
        if (permStr.IsNullOrWhiteSpace())
        {
            return await Task.FromResult(true);
        }

        if (userBelongsRoleIds.IsNullOrWhiteSpace())
        {
            return await Task.FromResult(false);
        }

        var roleIds = userBelongsRoleIds.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.Trim()));

        var cache = await cacheProvider.GetAsync<List<PermissionInfo>>(GeneralConsts.RoleMenuCodesCacheKey);

        if (cache == null || cache.Value is null)
        {
            return await Task.FromResult(false);
        }

        var upperCodes = cache.Value.Where(x => roleIds.Contains(x.RoleId)).SelectMany(x => x.Perms.Select(y => y.ToUpper())).Distinct();
        if (upperCodes == null || !upperCodes.Any())
        {
            return await Task.FromResult(false);
        }

        var result = upperCodes.Intersect(requestPermissions.Select(x => x.ToUpper()));

        return await Task.FromResult(result.Any());
    }
}
