using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Extensions;
using Adnc.Infra.Common.Helper;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;
using Adnc.Maint.Application.Services.Caching;
using Adnc.Maint.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Services
{
    public class DictAppService : AbstractAppService, IDictAppService
    {
        private readonly IEfRepository<SysDict> _dictRepository;
        private readonly CacheService _cacheService;

        public DictAppService(IEfRepository<SysDict> dictRepository
            , CacheService cacheService)
        {
            _dictRepository = dictRepository;
            _cacheService = cacheService;
        }

        public async Task<AppSrvResult<long>> CreateAsync(DictCreationDto input)
        {
            var exists = (await _cacheService.GetAllDictsFromCacheAsync()).Exists(x => x.Name.EqualsIgnoreCase(input.Name));
            if (exists)
                return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

            var dists = new List<SysDict>();
            long id = IdGenerater.GetNextId();
            //var subDicts = GetSubDicts(id, input.DictValues);
            var dict = new SysDict { Id = id, Name = input.Name, Value = input.Value, Ordinal = input.Ordinal, Pid = 0 };

            dists.Add(dict);
            input.Children?.ForEach(x =>
            {
                dists.Add(new SysDict
                {
                    Id = IdGenerater.GetNextId()
                    ,
                    Pid = id
                    ,
                    Name = x.Name
                    ,
                    Value = x.Value
                    ,
                    Ordinal = x.Ordinal
                });
            });

            await _dictRepository.InsertRangeAsync(dists);

            return id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id, DictUpdationDto input)
        {
            var exists = (await _cacheService.GetAllDictsFromCacheAsync()).Exists(x => x.Name.EqualsIgnoreCase(input.Name) && x.Id != id);
            if (exists)
                return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

            var dict = new SysDict { Name = input.Name, Value = input.Value, Id = id, Pid = 0, Ordinal = input.Ordinal };

            var subDicts = new List<SysDict>();
            input.Children?.ForEach(x =>
            {
                subDicts.Add(new SysDict
                {
                    Id = IdGenerater.GetNextId()
                    ,
                    Pid = id
                    ,
                    Name = x.Name
                    ,
                    Value = x.Value
                    ,
                    Ordinal = x.Ordinal
                });
            });

            // 这里需要事务处理
            await _dictRepository.UpdateAsync(dict, UpdatingProps<SysDict>(d => d.Name, d => d.Value, d => d.Ordinal));
            await _dictRepository.DeleteRangeAsync(d => d.Pid == dict.Id);
            if (subDicts?.Count > 0)
            {
                await _dictRepository.InsertRangeAsync(subDicts);
            }

            return AppSrvResult();
        }

        public async Task<AppSrvResult> DeleteAsync(long id)
        {
            await _dictRepository.DeleteRangeAsync(d => (d.Id == id) || (d.Pid == id));
            return AppSrvResult();
        }

        public async Task<DictDto> GetAsync(long id)
        {
            var dictDto = (await _cacheService.GetAllDictsFromCacheAsync()).Where(x => x.Id == id).FirstOrDefault();

            if (dictDto == null)
                return default;

            dictDto.Children = (await _cacheService.GetAllDictsFromCacheAsync()).Where(x => x.Pid == id).ToList();

            return dictDto;
        }

        public async Task<List<DictDto>> GetListAsync(DictSearchDto search)
        {
            var result = new List<DictDto>();

            Expression<Func<DictDto, bool>> whereCondition = x => true;
            if (search.Name.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(search.Name));
            }

            var dicts = (await _cacheService.GetAllDictsFromCacheAsync()).Where(whereCondition.Compile()).OrderBy(d => d.Ordinal).ToList();
            if (dicts.Any())
            {
                result = dicts.Where(d => d.Pid == 0).OrderBy(d => d.Ordinal).ToList();
                foreach (var item in result)
                {
                    var subDicts = dicts.Where(x => x.Pid == item.Id).OrderBy(x => x.Ordinal).ToList();
                    item.Children = subDicts;
                }
            }
            return result;
        }
    }
}