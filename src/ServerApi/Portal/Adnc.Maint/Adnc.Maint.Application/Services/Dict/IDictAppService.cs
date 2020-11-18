using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace  Adnc.Maint.Application.Services
{
    public interface IDictAppService : IAppService
    {
        Task<List<DictDto>> GetList(DictSearchDto searchDto);

        Task<DictDto> Get(long id);

        //Task<DictDto> GetInculdeSubs(long id);

        [OpsLog(LogName = "新增/修改字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task Save(DictSaveInputDto saveDto);

        [OpsLog(LogName = "删除字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task Delete(long Id);
    }
}
