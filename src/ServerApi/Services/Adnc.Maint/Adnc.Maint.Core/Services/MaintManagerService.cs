using Adnc.Core.Shared;
using Adnc.Core.Shared.Interceptors;
using Adnc.Core.Shared.IRepositories;
using Adnc.Maint.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Maint.Core.Services
{
    public class MaintManager : AbstractCoreService
    {
        private readonly IEfRepository<SysDict> _dictRepository;

        public MaintManager(IEfRepository<SysDict> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        [UnitOfWork]
        public virtual async Task UpdateDictsAsync(SysDict dict, List<SysDict> subDicts, CancellationToken cancellationToken = default)
        {
            await _dictRepository.UpdateAsync(dict, UpdatingProps<SysDict>(d => d.Name, d => d.Value, d => d.Ordinal), cancellationToken);
            await _dictRepository.DeleteRangeAsync(d => d.Pid == dict.Id, cancellationToken);
            if (subDicts?.Count > 0)
            {
                await _dictRepository.InsertRangeAsync(subDicts, cancellationToken);
            }
        }
    }
}