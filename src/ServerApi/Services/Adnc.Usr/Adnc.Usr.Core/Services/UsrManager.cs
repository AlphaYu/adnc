﻿using Adnc.Core.Shared;
using Adnc.Core.Shared.Interceptors;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Helper;
using Adnc.Usr.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Usr.Core.Services
{
    public class UsrManager : AbstractCoreService
    {
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly IEfRepository<SysDept> _deptRepository;

        public UsrManager(IEfRepository<SysRelation> relationRepository
            , IEfRepository<SysDept> deptRepository
            )
        {
            _relationRepository = relationRepository;
            _deptRepository = deptRepository;
        }

        [UnitOfWork]
        public virtual async Task SetRolePermissonAsync(long roleId, long[] permissionIds, CancellationToken cancellationToken = default)
        {
            await _relationRepository.DeleteRangeAsync(x => x.RoleId == roleId, cancellationToken);

            var relations = new List<SysRelation>();
            foreach (var permissionId in permissionIds)
            {
                relations.Add(
                    new SysRelation
                    {
                        Id = IdGenerater.GetNextId(),
                        RoleId = roleId,
                        MenuId = permissionId
                    }
                );
            }
            await _relationRepository.InsertRangeAsync(relations, cancellationToken);
        }

        [UnitOfWork]
        public virtual async Task UpdateDeptAsync(string oldDeptPids, SysDept dept, CancellationToken cancellationToken = default)
        {
            var updatingProps = UpdatingProps<SysDept>(x => x.FullName, x => x.SimpleName, x => x.Tips, x => x.Ordinal, x => x.Pid, x => x.Pids);

            await _deptRepository.UpdateAsync(dept, updatingProps);

            //zz.efcore 不支持
            //await _deptRepository.UpdateRangeAsync(d => d.Pids.Contains($"[{dept.ID}]"), c => new SysDept { Pids = c.Pids.Replace(oldDeptPids, dept.Pids) });
            var originalDeptPids = $"{oldDeptPids}[{dept.Id}],";
            var nowDeptPids = $"{dept.Pids}[{dept.Id}],";

            var subDepts = await _deptRepository
                                 .Where(d => d.Pids.StartsWith(originalDeptPids))
                                 .Select(d => new { d.Id, d.Pids })
                                 .ToListAsync();

            foreach (var c in subDepts)
            {
                await _deptRepository.UpdateAsync(new SysDept { Id = c.Id, Pids = c.Pids.Replace(originalDeptPids, nowDeptPids) }, UpdatingProps<SysDept>(c => c.Pids));
            };
        }
    }
}