using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Maint.Core.Services
{
    public class MaintManagerService : ICoreService
    {
        private readonly IEfRepository<SysDict> _dictRepository;

        public MaintManagerService(IEfRepository<SysDict> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        [UnitOfWork]
        public virtual async Task UpdateDicts(SysDict dict, List<SysDict> subDicts, CancellationToken cancellationToken = default)
        {
            await _dictRepository.UpdateAsync(dict, d => d.Name, d => d.Tips);
            await _dictRepository.DeleteRangeAsync(d => d.Pid == dict.Id);
            if (subDicts != null && subDicts.Count() > 0)
            {
                await _dictRepository.InsertRangeAsync(subDicts);
            }
        }
    }
}
