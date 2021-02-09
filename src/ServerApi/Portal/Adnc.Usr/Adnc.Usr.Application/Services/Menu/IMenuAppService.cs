using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Usr.Application.Services
{
    public interface IMenuAppService : IAppService
    {
        Task<AppSrvResult<List<MenuNodeDto>>> GetlistAsync();

        Task<AppSrvResult<List<MenuRouterDto>>> GetMenusForRouterAsync(long[] roleIds);

        Task<AppSrvResult<dynamic>> GetMenuTreeListByRoleIdAsync(long roleId);

        [OpsLog(LogName = "新增菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult<long>> CreateAsync(MenuCreationDto input);

        [OpsLog(LogName = "修改菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> UpdateAsync(long id,MenuUpdationDto input);

        [OpsLog(LogName = "删除菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> DeleteAsync(long id);
    }
}
