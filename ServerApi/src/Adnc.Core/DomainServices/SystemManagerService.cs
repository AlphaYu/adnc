using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Common;
using Adnc.Common.Extensions;
using Adnc.Common.Helper;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;

namespace Adnc.Core.DomainServices
{
    public class SystemManagerService : ISystemManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEfRepository<SysUser> _userRepository;
        private readonly IEfRepository<SysUserFinance> _financeRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly IEfRepository<SysDept> _deptRepository;
        private readonly IEfRepository<SysDict> _dictRepository;
        private readonly IMenuRepository _menuRepository;

        public SystemManagerService(IUnitOfWork unitOfWork
            , IEfRepository<SysUser> userRepository
            , IEfRepository<SysUserFinance> financeRepository
            , IEfRepository<SysRelation> relationRepository
            , IMenuRepository menuRepository
            , IEfRepository<SysDept> deptRepository
            , IEfRepository<SysDict> dictRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _financeRepository = financeRepository;
            _relationRepository = relationRepository;
            _menuRepository = menuRepository;
            _deptRepository = deptRepository;
            _dictRepository = dictRepository;
        }

        public async Task<int> AddUser(SysUser user, CancellationToken cancellationToken = default)
        {
            int result = 0;

            var tran = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _userRepository.InsertAsync(user, cancellationToken);
                await _financeRepository.InsertAsync(new SysUserFinance { ID = user.ID, Amount = 0.00M }, cancellationToken);

                await tran.CommitAsync();

                result = 1;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();

                throw ex;
            }

            return await Task.FromResult(result);
        }

        public async Task<int> DeleteMenu(SysMenu menu, CancellationToken cancellationToken = default)
        {
            int result = 0;

            var tran = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _menuRepository.DeleteRangeAsync(x => x.PCodes.Contains($"[{menu.Code}]"));
                await _menuRepository.DeleteAsync(new[] { menu.ID });

                await tran.CommitAsync();

                result = 1;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();

                throw ex;
            }

            return await Task.FromResult(result);
        }

        public async Task<int> SaveRolePermisson(long roleId, long[] permissionIds, CancellationToken cancellationToken = default)
        {
            int result = 0;
            var tran = await _unitOfWork.BeginTransactionAsync();          
            try
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
                await _relationRepository.InsertAsync(relations);

                await tran.CommitAsync();

                result = 1;
            }
            catch(Exception ex)
            {
                await tran.RollbackAsync();
                throw ex;
            }

            return await Task.FromResult(result);
        }

        public async Task<int> DeleteDept(long deptId, CancellationToken cancellationToken = default)
        {
            int result = 0;

            var tran = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _deptRepository.DeleteRangeAsync(x => x.Pids.Contains($"[{deptId}]"));
                await _deptRepository.DeleteAsync(new[] { deptId });

                await tran.CommitAsync();

                result = 1;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();

                throw ex;
            }

            return await Task.FromResult(result);
        }

        public async  Task<int> UpdateDicts(SysDict dict, IEnumerable<SysDict> subDicts, CancellationToken cancellationToken = default)
        {
            int result = 0;

            var tran = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _dictRepository.UpdateAsync(dict, d => d.Name, d => d.Tips);
                await _dictRepository.DeleteRangeAsync(d => d.Pid == dict.ID);
                if (subDicts != null && subDicts.Count() > 0)
                {
                    await _dictRepository.InsertAsync(subDicts);
                }

                await tran.CommitAsync();

                result = 1;
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();

                throw ex;
            }

            return await Task.FromResult(result);
        }
    }
}
