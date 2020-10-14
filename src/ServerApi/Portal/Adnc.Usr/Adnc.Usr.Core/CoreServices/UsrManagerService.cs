using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Common.Extensions;
using Adnc.Common.Helper;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;

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
                        ID = IdGeneraterHelper.GetNextId(IdGeneraterKey.PEMS, permissionIds.IndexOf(permissionId)),
                        RoleId = roleId,
                        MenuId = permissionId
                    }
                );
            }
            await _relationRepository.InsertRangeAsync(relations);
        }

        public async Task DeleteDept(long deptId, CancellationToken cancellationToken = default)
        {
            await _deptRepository.DeleteRangeAsync(x => x.Pids.Contains($"[{deptId}]"));
            await _deptRepository.DeleteAsync(new[] { deptId });
        }
    }
}
