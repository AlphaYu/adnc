using System.ComponentModel.DataAnnotations;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Core.Shared.Entities
{
    public abstract class EfEntity : Entity, IEfEntity<long>
    {
    }
}
