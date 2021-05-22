using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Core.Maint.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Extensions;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;
using Adnc.Maint.Core.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Services
{
    public class LogAppService : AbstractAppService, ILogAppService
    {
        private readonly IMongoRepository<SysOperationLog> _opsLogRepository;
        private readonly IMongoRepository<SysNloglog> _nlogLogRepository;
        private readonly IMongoRepository<SysLoginLog> _loginLogRepository;

        public LogAppService(IMongoRepository<SysOperationLog> opsLogRepository
            , IMongoRepository<SysLoginLog> loginLogRepository
            , IMongoRepository<SysNloglog> nlogLogRepository)
        {
            _opsLogRepository = opsLogRepository;
            _loginLogRepository = loginLogRepository;
            _nlogLogRepository = nlogLogRepository;
        }

        public async Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto)
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

            var result = Mapper.Map<PageModelDto<LoginLogDto>>(pagedModel);

            return result;
        }

        public async Task<PageModelDto<OpsLogDto>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto)
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

            var result = Mapper.Map<PageModelDto<OpsLogDto>>(pagedModel);

            return result;
        }

        public async Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto)
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

            var result = Mapper.Map<PageModelDto<NlogLogDto>>(pagedModel);

            return result;
        }
    }
}