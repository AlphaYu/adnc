﻿using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Core.Services;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Helper;
using Adnc.Application.Shared.Services;
using Adnc.Usr.Application.Caching;
using Adnc.Usr.Application.Contracts.Services;

namespace Adnc.Usr.Application.Services
{
    public class DeptAppService : AbstractAppService, IDeptAppService
    {
        private readonly IEfRepository<SysDept> _deptRepository;
        private readonly UsrManager _usrManager;
        private readonly CacheService _cacheService;

        public DeptAppService(IEfRepository<SysDept> deptRepository
            , UsrManager usrManager
            , CacheService cacheService)
        {
            _deptRepository = deptRepository;
            _usrManager = usrManager;
            _cacheService = cacheService;
        }

        public async Task<AppSrvResult> DeleteAsync(long Id)
        {
            var dept = (await _cacheService.GetAllDeptsFromCacheAsync()).FirstOrDefault(x => x.Id == Id);
            var deletingPids = $"{dept.Pids}[{Id}],";
            await _deptRepository.DeleteRangeAsync(d => d.Pids.StartsWith(deletingPids) || d.Id == dept.Id);

            return AppSrvResult();
        }

        public async Task<AppSrvResult<long>> CreateAsync(DeptCreationDto input)
        {
            var isExists = (await _cacheService.GetAllDeptsFromCacheAsync()).Exists(x => x.FullName == input.FullName);
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

            var dept = Mapper.Map<SysDept>(input);
            dept.Id = IdGenerater.GetNextId();
            await this.SetDeptPids(dept);
            await _deptRepository.InsertAsync(dept);

            return dept.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input)
        {
            var allDepts = await _cacheService.GetAllDeptsFromCacheAsync();

            var oldDeptDto = allDepts.FirstOrDefault(x => x.Id == id);
            if (oldDeptDto.Pid == 0 && input.Pid > 0)
                return Problem(HttpStatusCode.BadRequest, "一级单位不能修改等级");

            var isExists = allDepts.Exists(x => x.FullName == input.FullName && x.Id != id);
            if (isExists)
                return Problem(HttpStatusCode.BadRequest, "该部门全称已经存在");

            var oldPids = oldDeptDto.Pids;

            var deptEnity = Mapper.Map<SysDept>(input);

            deptEnity.Id = id;

            if (oldDeptDto.Pid == input.Pid)
            {
                await _deptRepository.UpdateAsync(deptEnity, UpdatingProps<SysDept>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal));
            }
            else
            {
                await this.SetDeptPids(deptEnity);
                await _usrManager.UpdateDeptAsync(oldPids, deptEnity);
            }

            return AppSrvResult();
        }

        public async Task<List<DeptTreeDto>> GetTreeListAsync()
        {
            var result = new List<DeptTreeDto>();

            var depts = await _cacheService.GetAllDeptsFromCacheAsync();

            if (!depts.Any())
                return result;

            var allDeptNodes = Mapper.Map<List<DeptTreeDto>>(depts);

            var roots = allDeptNodes.Where(d => d.Pid == 0).OrderBy(d => d.Ordinal);
            foreach (var node in roots)
            {
                GetChildren(node, allDeptNodes);
                result.Add(node);
            }

            void GetChildren(DeptTreeDto currentNode, List<DeptTreeDto> allDeptNodes)
            {
                var childrenNodes = allDeptNodes.Where(d => d.Pid == currentNode.Id).OrderBy(d => d.Ordinal);
                if (childrenNodes.Count() == 0)
                    return;
                else
                {
                    currentNode.Children.AddRange(childrenNodes);
                    foreach (var node in childrenNodes)
                    {
                        GetChildren(node, allDeptNodes);
                    }
                }
            }
            return result;
        }

        private async Task<SysDept> SetDeptPids(SysDept sysDept)
        {

            if (sysDept.Pid.HasValue && sysDept.Pid.Value > 0)
            {
                var dept = (await _cacheService.GetAllDeptsFromCacheAsync()).FirstOrDefault(x => x.Id == sysDept.Pid.Value);
                string pids = dept?.Pids ?? "";
                sysDept.Pids = $"{pids}[{sysDept.Pid}],";
            }
            else
            {
                sysDept.Pid = 0;
                sysDept.Pids = "[0],";
            }
            return sysDept;
        }
    }
}
