using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;

namespace Adnc.Maint.Core.CoreServices
{
    public class MaintManagerService : IMaintManagerService
    {
        private readonly IEfRepository<SysDict> _dictRepository;

        public MaintManagerService(IEfRepository<SysDict> dictRepository)
        {
            _dictRepository = dictRepository;
        }

        public async Task UpdateDicts(SysDict dict, List<SysDict> subDicts, CancellationToken cancellationToken = default)
        {
            await _dictRepository.UpdateAsync(dict, d => d.Name, d => d.Tips);
            await _dictRepository.DeleteRangeAsync(d => d.Pid == dict.ID);
            if (subDicts != null && subDicts.Count() > 0)
            {
                await _dictRepository.InsertRangeAsync(subDicts);
            }
        }
    }
}
