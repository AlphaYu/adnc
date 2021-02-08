using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
    }
}
