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
        Task<List<MenuNodeDto>> Getlist();

        Task<List<RouterMenuDto>> GetMenusForRouter(long[] roleIds);

        Task<dynamic> GetMenuTreeListByRoleId(long roleId);

        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task Save(MenuSaveInputDto saveDto);

        [EasyCachingEvict(CacheKeys = new[] { EasyCachingConsts.MenuListCacheKey, EasyCachingConsts.MenuRelationCacheKey, EasyCachingConsts.MenuCodesCacheKey })]
        Task Delete(long Id);
    }
}
