using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IDeptAppService : IAppService
    {
        [EasyCachingAble(CacheKey = EasyCachingConsts.DetpListCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<List<DeptNodeDto>> GetList();

        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task Save(DeptSaveInputDto savetDto);

        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task Delete(long Id);
    }
}
