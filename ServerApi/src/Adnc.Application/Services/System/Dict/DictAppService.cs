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

namespace Adnc.Application.Services
{
    public class DictAppService : AppService, IDictAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysDict> _dictRepository;
        private readonly ISystemManagerService _systemManagerService;

        public DictAppService(IMapper mapper
            , IEfRepository<SysDict> dictRepository
            , ISystemManagerService systemManagerService)
        {
            _mapper = mapper;
            _dictRepository = dictRepository;
            _systemManagerService = systemManagerService;
        }

        public async Task Delete(long Id)
        {
            await _dictRepository.UpdateRangeAsync(d => (d.ID == Id) || (d.Pid == Id), d => new SysDict { IsDeleted = true });
        }

        public async Task<List<DictDto>> GetList(DictSearchDto searchDto)
        {
            List<DictDto> result = new List<DictDto>();

            Expression<Func<SysDict, bool>> whereCondition = x => true;
            if (!string.IsNullOrWhiteSpace(searchDto.Name))
            {
                whereCondition = whereCondition.And(x => x.Name.Contains(searchDto.Name));
            }

            var dicts = await _dictRepository.GetAll().Where(whereCondition).Select(d => d).OrderByDescending(d => d.CreateTime).ToListAsync();
            if (dicts.Any())
            {
                result = _mapper.Map<List<DictDto>>(dicts.Where(d => d.Pid == 0).OrderBy(d => d.Num));
                foreach (var item in result)
                {
                    var subDict = dicts.Where(d => d.Pid == item.ID).OrderBy(d => d.Num).Select(d => $"{d.Num}:{d.Name}");
                    item.Detail = string.Join(";", subDict);
                }

            }
            return result;
        }

        public async Task Save(DictSaveInputDto saveDto)
        {
            if (string.IsNullOrWhiteSpace(saveDto.DictName))
            {
                throw new BusinessException(new ErrorModel(ErrorCode.BadRequest,"请输入字典名称"));
            }

            //add
            if (saveDto.ID == 0)
            {
                //long Id = new Snowflake(1, 1).NextId();
                long Id = IdGeneraterHelper.GetNextId(IdGeneraterKey.DICT);
                var subDicts = GetSubDicts(Id, saveDto.DictValues);
                await _dictRepository.InsertRangeAsync(subDicts.Append(new SysDict { ID = Id, Pid = 0, Name = saveDto.DictName, Tips = saveDto.Tips,Num="0" }));
            }
            //update
            else
            {
                var dict = new SysDict { Name = saveDto.DictName, Tips = saveDto.Tips, ID = saveDto.ID, Pid = 0 };
                var subDicts = GetSubDicts(saveDto.ID, saveDto.DictValues);
                await _systemManagerService.UpdateDicts(dict, subDicts);
            }
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
                    ID = IdGeneraterHelper.GetNextId(IdGeneraterKey.DICT, Index)
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
