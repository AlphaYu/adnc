using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Helper;
using Adnc.Infra.IRepositories;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;
using Adnc.Maint.Application.Services.Caching;
using Adnc.Maint.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Services
{
    public class CfgAppService : AbstractAppService, ICfgAppService
    {
        private readonly IEfRepository<SysCfg> _cfgRepository;
        private readonly CacheService _cacheService;

        public CfgAppService(IEfRepository<SysCfg> cfgRepository
            , CacheService cacheService)
        {
            _cfgRepository = cfgRepository;
            _cacheService = cacheService;
        }

        public async Task<AppSrvResult<long>> CreateAsync(CfgCreationDto input)
        {
            var exist = (await _cacheService.GetAllCfgsFromCacheAsync()).Exists(c => c.Name.EqualsIgnoreCase(input.Name));
            if (exist)
                return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

            var cfg = Mapper.Map<SysCfg>(input);
            cfg.Id = IdGenerater.GetNextId();

            await _cfgRepository.InsertAsync(cfg);

            return cfg.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id, CfgUpdationDto input)
        {
            var exist = (await _cacheService.GetAllCfgsFromCacheAsync()).Exists(c => c.Name.EqualsIgnoreCase(input.Name) && c.Id != id);
            if (exist)
                return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

            var entity = Mapper.Map<SysCfg>(input);

            entity.Id = id;

            var updatingProps = UpdatingProps<SysCfg>(x => x.Name, x => x.Value, x => x.Description);

            await _cfgRepository.UpdateAsync(entity, updatingProps);

            return AppSrvResult();
        }

        public async Task<AppSrvResult> DeleteAsync(long id)
        {
            await _cfgRepository.DeleteAsync(id);
            return AppSrvResult();
        }

        public async Task<CfgDto> GetAsync(long id)
        {
            return (await _cacheService.GetAllCfgsFromCacheAsync()).Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<PageModelDto<CfgDto>> GetPagedAsync(CfgSearchPagedDto search)
        {
            Expression<Func<CfgDto, bool>> whereCondition = x => true;
            if (search.Name.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(search.Name));
            }
            if (search.Value.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Value.Contains(search.Value));
            }

            var allCfgs = await _cacheService.GetAllCfgsFromCacheAsync();

            var pagedCfgs = allCfgs.Where(whereCondition.Compile())
                                   .OrderByDescending(x => x.CreateTime)
                                   .Skip((search.PageIndex - 1) * search.PageSize)
                                   .Take(search.PageSize)
                                   .ToArray();

            var result = new PageModelDto<CfgDto>()
            {
                Data = pagedCfgs
                ,
                TotalCount = allCfgs.Count
                ,
                PageIndex = search.PageIndex
                ,
                PageSize = search.PageSize
            };

            return result;
        }
    }
}