using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Maint.Application.Contracts.Consts;
using Adnc.Maint.Application.Contracts.Dtos;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Contracts.Services
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public interface ICfgAppService : IAppService
    {
        /// <summary>
        /// 新增参数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "新增参数")]
        [CachingEvict(CacheKey = CachingConsts.CfgListCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(CfgCreationDto input);

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "修改参数")]
        [CachingEvict(CacheKey = CachingConsts.CfgListCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, CfgUpdationDto input);

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OpsLog(LogName = "删除参数")]
        [CachingEvict(CacheKey = CachingConsts.CfgListCacheKey)]
        Task<AppSrvResult> DeleteAsync(long id);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CfgDto> GetAsync(long id);

        /// <summary>
        /// 配置列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<PageModelDto<CfgDto>> GetPagedAsync(CfgSearchPagedDto search);
    }
}