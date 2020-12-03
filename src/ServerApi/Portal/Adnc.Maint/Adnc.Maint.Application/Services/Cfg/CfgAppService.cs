﻿using System;
using System.Net;
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

        public async Task<AppSrvResult> Delete(long Id)
        {
            //await _cfgRepository.UpdateAsync(new SysCfg { ID = Id, IsDeleted=true }, c => c.IsDeleted);
            await _cfgRepository.DeleteAsync(Id);
            return DefaultResult();
        }

        public async Task<AppSrvResult<PageModelDto<CfgDto>>> GetPaged(CfgSearchDto searchDto)
        {
            Expression<Func<CfgDto, bool>> whereCondition = x => true;
            if (searchDto.CfgName.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.CfgName.Contains(searchDto.CfgName));
            }
            if (searchDto.CfgValue.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.CfgValue.Contains(searchDto.CfgValue));
            }

            //var pagedModel = await _cfgRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, whereCondition, c => c.CreateTime, false);
            var allCfgs = await this.GetAllFromCache();

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

            return result;
        }

        public async Task<AppSrvResult<long>> Add(CfgSaveInputDto inputDto)
        {
            var exist = (await this.GetAllFromCache()).Exists(c => c.CfgName.EqualsIgnoreCase(inputDto.CfgName));
            if (exist)
                return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

            var cfg = _mapper.Map<SysCfg>(inputDto);
            cfg.ID = IdGenerater.GetNextId();

            await _cfgRepository.InsertAsync(cfg);

            return cfg.ID;
        }

        public async Task<AppSrvResult> Update(CfgSaveInputDto inputDto)
        {
            var exist = (await this.GetAllFromCache()).Exists(c => c.CfgName.EqualsIgnoreCase(inputDto.CfgName) && c.ID != inputDto.ID);
            if (exist)
                return Problem(HttpStatusCode.BadRequest, "参数名称已经存在");

            var enity = _mapper.Map<SysCfg>(inputDto);
            await _cfgRepository.UpdateAsync(enity);

            return DefaultResult();
        }

        public async Task<AppSrvResult<CfgDto>> Get(long id)
        {
            return (await this.GetAllFromCache()).Where(x => x.ID == id).FirstOrDefault();
        }

        private async Task<List<CfgDto>> GetAllFromCache()
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
