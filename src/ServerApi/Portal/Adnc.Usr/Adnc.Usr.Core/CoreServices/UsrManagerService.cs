using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Infr.Common.Helper;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Microsoft.EntityFrameworkCore.Internal;

namespace Adnc.Usr.Core.CoreServices
{
    public class UsrManagerService : IUsrManagerService
    {
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysUserFinance> _financeRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly IEfRepository<SysDept> _deptRepository;
        private readonly IEfRepository<SysMenu> _menuRepository;

        public UsrManagerService(IEfRepository<SysUser> userRepository
            , IEfRepository<SysUserFinance> financeRepository
            , IEfRepository<SysRelation> relationRepository
            , IEfRepository<SysMenu> menuRepository
            , IEfRepository<SysDept> deptRepository
            )
        {
            _userRepository = userRepository;
            _financeRepository = financeRepository;
            _relationRepository = relationRepository;
            _menuRepository = menuRepository;
            _deptRepository = deptRepository;
        }

        public async Task AddUser(SysUser user, CancellationToken cancellationToken = default)
        {
            await _userRepository.InsertAsync(user, cancellationToken);
            await _financeRepository.InsertAsync(new SysUserFinance { ID = user.ID, Amount = 0.00M }, cancellationToken);
        }

        public async Task DeleteMenu(SysMenu menu, CancellationToken cancellationToken = default)
        {
            await _menuRepository.DeleteRangeAsync(x => x.PCodes.Contains($"[{menu.Code}]"));
            await _menuRepository.DeleteAsync(new[] { menu.ID });
        }

        public async Task SaveRolePermisson(long roleId, long[] permissionIds, CancellationToken cancellationToken = default)
        {
            await _relationRepository.DeleteRangeAsync(x => x.RoleId == roleId);

            var relations = new List<SysRelation>();
            foreach (var permissionId in permissionIds)
            {
                relations.Add(
                    new SysRelation
                    {
                        ID = IdGenerater.GetNextId(),
                        RoleId = roleId,
                        MenuId = permissionId
                    }
                );
            }
            await _relationRepository.InsertRangeAsync(relations);
        }

        public async Task UpdateDept(string oldDeptPids, SysDept dept, CancellationToken cancellationToken = default)
        {
            await _deptRepository.UpdateAsync(dept);
            //zz.efcore 不支持
            //await _deptRepository.UpdateRangeAsync(d => d.Pids.Contains($"[{dept.ID}]"), c => new SysDept { Pids = c.Pids.Replace(oldDeptPids, dept.Pids) });
            var originalDeptPids = $"{oldDeptPids}[{dept.ID}],";
            var nowDeptPids = $"{dept.Pids}[{dept.ID}],";

            var subDepts = await _deptRepository.SelectAsync(d => new { d.ID, d.Pids }
                                                            , d => d.Pids.StartsWith(originalDeptPids)
                                                            );
            subDepts.ForEach(c =>
            {
                _deptRepository.UpdateAsync(new SysDept { ID = c.ID, Pids = c.Pids.Replace(originalDeptPids, nowDeptPids) }, c => c.Pids).Wait();
            });
        }
    }
}
