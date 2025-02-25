namespace Adnc.Demo.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 机构服务
    /// </summary>
    public interface IOrganizationAppService : IAppService
    {
        /// <summary>
        /// 新增机构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "新增机构")]
        [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey, CachingConsts.DetpTreeListCacheKey, CachingConsts.DetpSimpleTreeListCacheKey })]
        Task<AppSrvResult<long>> CreateAsync(OrganizationCreationDto input);

        /// <summary>
        /// 修改机构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperateLog(LogName = "修改机构")]
        [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey, CachingConsts.DetpTreeListCacheKey, CachingConsts.DetpSimpleTreeListCacheKey })]
        [UnitOfWork]
        Task<AppSrvResult> UpdateAsync(long id, OrganizationUpdationDto input);

        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperateLog(LogName = "删除机构")]
        [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey, CachingConsts.DetpTreeListCacheKey, CachingConsts.DetpSimpleTreeListCacheKey })]
        Task<AppSrvResult> DeleteAsync(long Id);

        /// <summary>
        /// 机构树结构
        /// </summary>
        /// <returns></returns>
        //[CachingAble(CacheKey = CachingConsts.DetpTreeListCacheKey, Expiration = CachingConsts.OneYear)]
        Task<List<OrganizationTreeDto>> GetTreeListAsync();
    }
}