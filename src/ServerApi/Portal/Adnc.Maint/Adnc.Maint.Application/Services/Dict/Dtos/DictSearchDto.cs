﻿using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.Application.Dtos
{
    /// <summary>
    /// 字典检索条件
    /// </summary>
    public class DictSearchDto : SearchDto
    {
        /// <summary>
        /// 字典名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        public string Value { get; set; }
    }
}
