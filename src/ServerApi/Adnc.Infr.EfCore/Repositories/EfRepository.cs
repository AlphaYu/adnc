using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Infr.EfCore.Repositories
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
