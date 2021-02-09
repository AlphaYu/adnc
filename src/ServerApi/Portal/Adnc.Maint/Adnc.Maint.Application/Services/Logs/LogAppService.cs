using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using AutoMapper;
using System.Collections.Generic;
using Adnc.Core.Maint.Entities;
using Adnc.Application.Shared.Dtos;
using Adnc.Core.Shared.IRepositories;
using Adnc.Maint.Core.Entities;
using Adnc.Maint.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Services;

namespace Adnc.Maint.Application.Services
{
    public class LogAppService : AppService, ILogAppService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<SysOperationLog> _opsLogRepository;
        private readonly IMongoRepository<SysNloglog> _nlogLogRepository;
        private readonly IMongoRepository<SysLoginLog> _loginLogRepository;

        public LogAppService(IMapper mapper,
            IMongoRepository<SysOperationLog> opsLogRepository
            , IMongoRepository<SysLoginLog> loginLogRepository
            , IMongoRepository<SysNloglog> nlogLogRepository)
        {
            _mapper = mapper;
            _opsLogRepository = opsLogRepository;
            _loginLogRepository = loginLogRepository;
            _nlogLogRepository = nlogLogRepository;
        }

        public async Task<AppSrvResult<PageModelDto<LoginLogDto>>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto)
        {
            var builder = Builders<SysLoginLog>.Filter;
            var filters = new List<FilterDefinition<SysLoginLog>>();

            if (searchDto.BeginTime.HasValue)
            {
                filters.Add(builder.Gte(l => l.CreateTime, searchDto.BeginTime));
            }

            if (searchDto.EndTime.HasValue)
            {
                filters.Add(builder.Lte(l => l.CreateTime, searchDto.EndTime));
            }

            if (searchDto.Account.IsNotNullOrWhiteSpace())
            {
                filters.Add(builder.Eq(l => l.Account, searchDto.Account));
            }

            if (searchDto.Method.IsNotNullOrWhiteSpace())
            {
                filters.Add(builder.Eq(l => l.Device, searchDto.Device));
            }

            var filter = filters.Count > 0 ? builder.And(filters) : builder.Where(x => true);

            var pagedModel = await _loginLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filter, x => x.CreateTime, false);

            var result = _mapper.Map<PageModelDto<LoginLogDto>>(pagedModel);

            return result;
        }


        public async Task<AppSrvResult<PageModelDto<OpsLogDto>>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto)
        {
            var builder = Builders<SysOperationLog>.Filter;
            var filters = new List<FilterDefinition<SysOperationLog>>();

            if (searchDto.BeginTime.HasValue)
            {
                filters.Add(builder.Gte(l => l.CreateTime, searchDto.BeginTime));
            }

            if (searchDto.EndTime.HasValue)
            {
                filters.Add(builder.Lte(l => l.CreateTime, searchDto.EndTime));
            }

            if (searchDto.Account.IsNotNullOrWhiteSpace())
            {
                filters.Add(builder.Eq(l => l.Account, searchDto.Account));
            }

            if (searchDto.Method.IsNotNullOrWhiteSpace())
            {
                filters.Add(builder.Eq(l => l.Method, searchDto.Method));
            }

            var filter = filters.Count > 0 ? builder.And(filters) : builder.Where(x => true);

            var pagedModel = await _opsLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filter, x => x.CreateTime, false);

            var result = _mapper.Map<PageModelDto<OpsLogDto>>(pagedModel);

            return result;
        }

        public async Task<AppSrvResult<PageModelDto<NlogLogDto>>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto)
        {
            var builder = Builders<SysNloglog>.Filter;
            var filters = new List<FilterDefinition<SysNloglog>>();

            if (searchDto.BeginTime.HasValue)
            {
                filters.Add(builder.Gte(l => l.Date, searchDto.BeginTime));
            }

            if (searchDto.EndTime.HasValue)
            {
                filters.Add(builder.Lte(l => l.Date, searchDto.EndTime));
            }

            //if (!string.IsNullOrWhiteSpace(searchDto.Account))
            //{
            //    filters.Add(builder.Eq(l => l.Properties., searchDto.Account));
            //}

            if (searchDto.Method.IsNotNullOrWhiteSpace())
            {
                filters.Add(builder.Eq(l => l.Properties.Method, searchDto.Method));
            }

            var filter = filters.Count > 0 ? builder.And(filters) : builder.Where(x => true);

            var pagedModel = await _nlogLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filter, x => x.Date, false);

            var result = _mapper.Map<PageModelDto<NlogLogDto>>(pagedModel);

            return result;
        }
    }
}
