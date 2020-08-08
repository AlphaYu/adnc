using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IMenuAppService : IAppService
    {
        [EasyCachingAble(CacheKey = EasyCachingConsts.MenuListCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<List<MenuNodeDto>> Getlist();

        [EasyCachingAble(CacheKey = EasyCachingConsts.MenuRouterCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<List<RouterMenuDto>> GetMenusForRouter();

        Task<dynamic> GetMenuTreeListByRoleId(long roleId);

        [EasyCachingAble(CacheKeyPrefix = EasyCachingConsts.MenuKesPrefix, Expiration = EasyCachingConsts.OneYear)]
        Task<List<MenuDto>> GetMenusByRoleIds(long[] roleIds);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.MenuKesPrefix, IsAll = true)]
        Task Save(MenuSaveInputDto saveDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.MenuKesPrefix, IsAll = true)]
        Task Delete(long Id);
    }
}
