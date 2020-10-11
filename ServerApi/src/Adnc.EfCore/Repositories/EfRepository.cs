using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.IRepositories;

namespace  Adnc.Infr.EfCore.Repositories
{
    public class EfRepository<TEntity> : BaseRepository<AdncDbContext, TEntity>, IEfRepository<TEntity>
      where TEntity : EfEntity
    {
        public EfRepository(AdncDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
