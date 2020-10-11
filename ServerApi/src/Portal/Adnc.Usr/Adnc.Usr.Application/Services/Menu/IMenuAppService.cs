using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Common.Consts;

namespace Adnc.Usr.Application.Services
{
    public interface IMenuAppService : IAppService
    {
        Task<List<MenuNodeDto>> Getlist();

        Task<List<RouterMenuDto>> GetMenusForRouter(long[] roleIds);

        Task<dynamic> GetMenuTreeListByRoleId(long roleId);

        [OpsLog(LogName = "新增/修改菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task Save(MenuSaveInputDto saveDto);

        [OpsLog(LogName = "删除菜单")]
        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task Delete(long Id);
    }
}
