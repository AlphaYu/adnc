using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Application.Dtos;
using Adnc.Core.Entities;

namespace Adnc.Application.Services
{
    public interface ICfgAppService : IAppService
    {
        [EasyCachingAble(CacheKeyPrefix = EasyCachingConsts.CfgListCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<PageModelDto<CfgDto>> GetPaged(CfgSearchDto searchDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.CfgListCacheKey, IsAll = true)]
        Task Save(CfgSaveInputDto saveInputDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.CfgListCacheKey, IsAll = true)]
        Task Delete(long Id);
    }
}
