using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Entities;
using Adnc.Infra.IRepositories;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Services
{
    public class LogAppService : AbstractAppService, ILogAppService
    {
        private readonly IMongoRepository<OperationLog> _opsLogRepository;
        private readonly IMongoRepository<LoggingLog> _nlogLogRepository;
        private readonly IMongoRepository<LoginLog> _loginLogRepository;

        public LogAppService(IMongoRepository<OperationLog> opsLogRepository
            , IMongoRepository<LoginLog> loginLogRepository
            , IMongoRepository<LoggingLog> nlogLogRepository)
        {
            _opsLogRepository = opsLogRepository;
            _loginLogRepository = loginLogRepository;
            _nlogLogRepository = nlogLogRepository;
        }

        public async Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto)
        {
            var builder = Builders<LoginLog>.Filter;
            var filters = new List<FilterDefinition<LoginLog>>();

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

            var result = Mapper.Map<PageModelDto<LoginLogDto>>(pagedModel);

            return result;
        }

        public async Task<PageModelDto<OpsLogDto>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto)
        {
            var builder = Builders<OperationLog>.Filter;
            var filters = new List<FilterDefinition<OperationLog>>();

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

            var result = Mapper.Map<PageModelDto<OpsLogDto>>(pagedModel);

            return result;
        }

        public async Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto)
        {
            var builder = Builders<LoggingLog>.Filter;
            var filters = new List<FilterDefinition<LoggingLog>>();

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

            if (searchDto.Level.IsNotNullOrWhiteSpace())
            {
                filters.Add(builder.Eq(l => l.Level, searchDto.Level));
            }

            var filter = filters.Count > 0 ? builder.And(filters) : builder.Where(x => true);

            var pagedModel = await _nlogLogRepository.PagedAsync(searchDto.PageIndex, searchDto.PageSize, filter, x => x.Date, false);

            var result = Mapper.Map<PageModelDto<NlogLogDto>>(pagedModel);

            return result;
        }
    }
}