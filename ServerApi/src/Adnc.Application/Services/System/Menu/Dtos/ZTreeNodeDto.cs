using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// ZTree节点
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class ZTreeNodeDto<TKey, TData>
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public TKey ID { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public TKey PID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool Open { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 节点数据
        /// </summary>
        public TData Data { get; set; }

        public static ZTreeNodeDto<TKey, TData> CreateParent()
        {
            ZTreeNodeDto<TKey, TData> node = new ZTreeNodeDto<TKey, TData>
            {
                Checked = true,
                ID = default(TKey),
                Name = "顶级",
                Open = true,
                PID =default(TKey)
            };

            return node;
        }
    }
}
