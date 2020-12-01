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
        Task<AppSrvResult<List<MenuNodeDto>>> Getlist();

        Task<AppSrvResult<List<RouterMenuDto>>> GetMenusForRouter(long[] roleIds);

        Task<AppSrvResult<dynamic>> GetMenuTreeListByRoleId(long roleId);

        [OpsLog(LogName = "新增菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult<long>> Add(MenuSaveInputDto saveDto);

        [OpsLog(LogName = "修改菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> Update(MenuSaveInputDto saveDto);

        [OpsLog(LogName = "删除菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task<AppSrvResult> Delete(long Id);
    }
}
