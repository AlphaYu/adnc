using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EasyCaching.Core;
using AutoMapper;
using Adnc.Maint.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Infr.Common.Helper;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared;

namespace  Adnc.Maint.Application.Services
{
    public class CfgAppService : AppService, ICfgAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysCfg> _cfgRepository;
        private readonly IRedisCachingProvider _redis;
        private readonly IHybridCachingProvider _cache;

        public CfgAppService(IMapper mapper
            , IEfRepository<SysCfg> cfgRepository
            , IHybridProviderFactory hybridProviderFactory)
        {
            _mapper = mapper;
            _cfgRepository = cfgRepository;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
        }

        public async Task Delete(long Id)
        {
            await _cfgRepository.UpdateAsync(new SysCfg { ID = Id, IsDeleted=true }, c => c.IsDeleted);
        }

        public async Task<PageModelDto<CfgDto>> GetPaged(CfgSearchDto searchDto)
        {
            //var result = new PageModelDto<CfgDto>();

            Expression<Func<CfgDto, bool>> whereCondition = x => true;
            if (!string.IsNullOrWhiteSpace(searchDto.CfgName))
            {
                whereCondition = whereCondition.And(x => x.CfgName.Contains(searchDto.CfgName));
            }
            if (!string.IsNullOrWhiteSpace(searchDto.CfgValue))
            {
                whereCondition = whereCondition.And(x => x.CfgValue.Contains(searchDto.CfgValue));
            }

            //var pagedModel = await _cfgRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, whereCondition, c => c.CreateTime, false);
            var allCfgs = await this.GetAll();

            var pagedCfgs = allCfgs.Where(whereCondition.Compile())
                                   .OrderByDescending(x => x.CreateTime)
                                   .Skip((searchDto.PageIndex - 1) * searchDto.PageSize)
                                   .Take(searchDto.PageSize)
                                   .ToArray();

            var result = new PageModelDto<CfgDto>()
            {
                Count = pagedCfgs.Count()
                ,
                Data = pagedCfgs
                ,
                TotalCount = allCfgs.Count
                ,
                PageIndex = searchDto.PageIndex
                ,
                PageSize = searchDto.PageSize
                ,
                PageCount = ((allCfgs.Count + searchDto.PageSize - 1) / searchDto.PageSize)
            };

            //result = _mapper.Map<PageModelDto<CfgDto>>(pagedModel);

            return result;
        }

        public async Task Save(CfgSaveInputDto inputDto)
        {
            if (string.IsNullOrWhiteSpace(inputDto.CfgName))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.BadRequest, "请输入参数名称"));
            }

            if (string.IsNullOrWhiteSpace(inputDto.CfgValue))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.BadRequest, "请输入参数值"));
            }

            //add
            if (inputDto.ID == 0)
            {
                var exist = await _cfgRepository.ExistAsync(c => c.CfgName == inputDto.CfgName);
                if (exist)
                    throw new BusinessException(new ErrorModel(ErrorCode.BadRequest, "参数名称已经存在"));

                var enity = _mapper.Map<SysCfg>(inputDto);

                //Vue处理大数字有问题，暂时不用Snowflake算法,以后完善。
                //enity.ID = new Snowflake(1, 1).NextId();
                enity.ID = IdGenerater.GetNextId();

                await _cfgRepository.InsertAsync(enity);
            }
            //update
            else
            {
                var exist = await _cfgRepository.ExistAsync(c => c.CfgName == inputDto.CfgName && c.ID != inputDto.ID);
                if (exist)
                    throw new BusinessException(new ErrorModel(ErrorCode.BadRequest, "参数名称已经存在"));

                var enity = _mapper.Map<SysCfg>(inputDto);

                await _cfgRepository.UpdateAsync(enity);
            }
        }

        public async Task<CfgDto> Get(long id)
        {
            return (await this.GetAll()).Where(x => x.ID == id).FirstOrDefault();
        }

        private async Task<List<CfgDto>> GetAll()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.CfgListCacheKey, async () =>
            {
                var allCfgs = await _cfgRepository.GetAll().ToListAsync();
                return _mapper.Map<List<CfgDto>>(allCfgs);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }
    }
}
