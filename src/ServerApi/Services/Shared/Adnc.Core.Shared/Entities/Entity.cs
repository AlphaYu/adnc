using System.ComponentModel.DataAnnotations;

namespace Adnc.Core.Shared.Entities
{
    public class Entity : IEntity<long>
    {
        [Key]
        public long Id { get; set; }
    }
}
