using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Core.Entities;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IDictAppService : IAppService
    {
        [EasyCachingAble(CacheKeyPrefix = EasyCachingConsts.DictListCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<List<DictDto>> GetList(DictSearchDto searchDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.DictListCacheKey,IsAll=true)]
        Task<int> Save(DictSaveInputDto saveDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.DictListCacheKey, IsAll = true)]
        Task<int> Delete(long Id);
    }
}
