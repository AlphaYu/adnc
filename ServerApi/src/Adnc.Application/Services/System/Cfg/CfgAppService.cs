using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Common;
using Adnc.Common.Extensions;
using Adnc.Common.Helper;
using Adnc.Common.Models;
using Adnc.Core.DomainServices;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;
using EasyCaching.Core;

namespace Adnc.Application.Services
{
    public class CfgAppService : AppService, ICfgAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysCfg> _cfgRepository;
        private readonly ISystemManagerService _systemManagerService;
        private readonly IRedisCachingProvider _redis;

        public CfgAppService(IMapper mapper
            , IEfRepository<SysCfg> cfgRepository
            , ISystemManagerService systemManagerService
            , IEasyCachingProviderFactory factory)
        {
            _mapper = mapper;
            _cfgRepository = cfgRepository;
            _systemManagerService = systemManagerService;
            _redis = factory.GetRedisProvider(EasyCachingConsts.RemoteCaching);
        }

        public async Task Delete(long Id)
        {
            await _cfgRepository.UpdateAsync(new SysCfg { ID = Id, IsDeleted=true }, c => c.IsDeleted);
        }

        public async Task<PageModelDto<CfgDto>> GetPaged(CfgSearchDto searchDto)
        {
            PageModelDto<CfgDto> result = new PageModelDto<CfgDto>();

            Expression<Func<SysCfg, bool>> whereCondition = x => true;
            if (!string.IsNullOrWhiteSpace(searchDto.CfgName))
            {
                whereCondition = whereCondition.And(x => x.CfgName.Contains(searchDto.CfgName));
            }
            if (!string.IsNullOrWhiteSpace(searchDto.CfgValue))
            {
                whereCondition = whereCondition.And(x => x.CfgValue.Contains(searchDto.CfgValue));
            }

            var pagedModel = await _cfgRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, whereCondition, c => c.CreateTime, false);
            result = _mapper.Map<PageModelDto<CfgDto>>(pagedModel);

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
                enity.ID = IdGeneraterHelper.GetNextId(IdGeneraterKey.CFG);

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
    }
}
