using Adnc.Infra.Entities;
using System.Collections.Generic;

namespace Adnc.Usr.Entities
{
    /// <summary>
    /// 部门
    /// </summary>
    public class SysDept : EfFullAuditEntity
    {
        //private ICollection<SysUser> _users;
        //private Action<object, string> LazyLoader { get; set; }
        //private SysDept(Action<object, string> lazyLoader)
        //{
        //	LazyLoader = lazyLoader;
        //}
        //public virtual ICollection<SysUser> Users
        //{
        //    //get => LazyLoader.Load(this, ref _users);
        //    //set => _users = value;
        //}

        public string FullName { get; set; }

        public int Ordinal { get; set; }

        public long? Pid { get; set; }

        public string Pids { get; set; }

        public string SimpleName { get; set; }

        public string Tips { get; set; }

        public int? Version { get; set; }

        public virtual ICollection<SysUser> Users { get; set; }
    }
}