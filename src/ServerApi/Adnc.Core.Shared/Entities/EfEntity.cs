using System.ComponentModel.DataAnnotations;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Core.Shared.Entities
{
    public abstract class EfEntity : IEfEntity<long>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Key]
        public long Id { get; set; }
        //private long _id;
        //public virtual long Id
        //{
        //    get { return _id; }
        //    set
        //    {
        //        if (_id > 0)
        //            throw new AdncArgumentException("Id不能被修改", nameof(Id));
        //        _id = Checker.GTZero(value, nameof(Id));
        //    }
        //}
    }
}
