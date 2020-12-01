using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EasyCaching.Core;
using Adnc.Maint.Application.Dtos;
using Adnc.Infr.Common.Extensions;
using Adnc.Infr.Common.Helper;
using Adnc.Maint.Core.CoreServices;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Services;

namespace  Adnc.Maint.Application.Services
{
    public class DictAppService : AppService, IDictAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysDict> _dictRepository;
        private readonly IMaintManagerService _maintManagerService;
        private readonly IHybridCachingProvider _cache;

        public DictAppService(IMapper mapper
            , IEfRepository<SysDict> dictRepository
            , IMaintManagerService maintManagerService
            , IHybridProviderFactory hybridProviderFactory)
        {
            _mapper = mapper;
            _dictRepository = dictRepository;
            _maintManagerService = maintManagerService;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
        }

        public async Task<AppSrvResult> Delete(long Id)
        {
            await _dictRepository.DeleteRangeAsync(d => (d.ID == Id) || (d.Pid == Id));
            return DefaultResult();
        }

        public async Task<AppSrvResult<List<DictDto>>> GetList(DictSearchDto searchDto)
        {
            var result = new List<DictDto>();

            Expression<Func<DictDto, bool>> whereCondition = x => true;
            if (searchDto.Name.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchDto.Name));
            }

            var dicts = (await this.GetAllFromCache()).Where(whereCondition.Compile()).OrderBy(d => d.Num).ToList();
            if (dicts.Any())
            {
                result = dicts.Where(d => d.Pid == 0).OrderBy(d => d.Num).ToList();
                foreach (var item in result)
                {
                    var subDict = dicts.Where(d => d.Pid == item.ID).OrderBy(d => d.Num).Select(d => $"{d.Num}:{d.Name}");
                    item.Detail = string.Join(";", subDict);
                }

            }
            return result;
        }

        public async Task<AppSrvResult<long>> Add(DictSaveInputDto saveDto)
        {
            var exists = (await GetAllFromCache()).Exists(x => x.Name.EqualsIgnoreCase(saveDto.DictName));
            if (exists)
                return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

            long Id = IdGenerater.GetNextId();
            var subDicts = GetSubDicts(Id, saveDto.DictValues);
            await _dictRepository.InsertRangeAsync(subDicts.Append(new SysDict { ID = Id, Pid = 0, Name = saveDto.DictName, Tips = saveDto.Tips, Num = "0" }));

            return Id;
        }

        public async Task<AppSrvResult> Update(DictSaveInputDto saveDto)
        {
            var exists = (await GetAllFromCache()).Exists(x => x.Name.EqualsIgnoreCase(saveDto.DictName) && x.ID != saveDto.ID);
            if (exists)
                return Problem(HttpStatusCode.BadRequest, "字典名字已经存在");

            var dict = new SysDict { Name = saveDto.DictName, Tips = saveDto.Tips, ID = saveDto.ID, Pid = 0 };
            var subDicts = GetSubDicts(saveDto.ID, saveDto.DictValues);
            await _maintManagerService.UpdateDicts(dict, subDicts);

            return DefaultResult();
        }

        public async Task<AppSrvResult<DictDto>> Get(long id)
        {
            var dictDto = (await this.GetAllFromCache()).Where(x => x.ID == id).FirstOrDefault();

            if (dictDto == null)
                return Problem(HttpStatusCode.NotFound, "没有找到");

            dictDto.Children = (await this.GetAllFromCache()).Where(x => x.Pid == dictDto.ID).ToList();

            return dictDto;
        }

        private async Task<List<DictDto>> GetAllFromCache()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.DictListCacheKey, async () =>
            {
                var allDicts = await _dictRepository.GetAll().ToListAsync();
                return _mapper.Map<List<DictDto>>(allDicts);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        private List<SysDict> GetSubDicts(long pid, string dictValues)
        {
            List<SysDict> subDicts = new List<SysDict>();
            if (!string.IsNullOrWhiteSpace(dictValues))
            {
                //var snowflake = new Snowflake(1, 1);
                var values = dictValues.Split(";", StringSplitOptions.RemoveEmptyEntries);
                subDicts = values.Select((s,Index) => new SysDict
                {
                    //ID = snowflake.NextId()
                    ID = IdGenerater.GetNextId()
                    ,
                    Pid = pid
                    ,
                    Name = s.Split(":", StringSplitOptions.RemoveEmptyEntries)[1]
                    ,
                    Num = s.Split(":", StringSplitOptions.RemoveEmptyEntries)[0]
                }).ToList();
            }
            return subDicts;
        }
    }
}
